using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using PetFinder.Data;
using PetFinder.Data.Models;
using PetFinder.Infrastructure;
using PetFinder.Services.Pets;
using PetFinder.Services.SearchPosts;
using PetFinder.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using static PetFinder.Tests.Data.SearchPostsData;
using static PetFinder.Tests.Data.CitiesData;
using static PetFinder.Tests.Data.PetData;

namespace PetFinder.Tests.Services
{
    public class SearchPostServiceTest
    {
        private SearchPostService searchPostService;
        private IPetService petService;
        private readonly ApplicationDbContext database;
        private IMapper mapper;

        public SearchPostServiceTest()
        {
            this.database = DatabaseMock.Instance;
            this.mapper = MapperMock.Mapper;
            this.petService = new PetService(database, mapper);
            this.searchPostService = new SearchPostService(database, petService, mapper);
        }

        [Theory]
        [InlineData("Test", "Description", "Found", 1, null, "2", "Jessy", "imageUrl", 1, 2, 1, "test", "1234")]
        [InlineData("12", "Some text", "Lost", 1, "12/05/2021", "2", "Jessy", "imageUrl", 1, 2, 1, "test", "089844524")]
        public void CreateShouldWorkWhenDataIsPassedCorrectly(string title, string description, string type, int cityId, string dateLostFound,
            string petId, string petName, string imageUrl, int speciesId, int sizeId, int? ownerId, string userId, string phoneNumber)
        {
            DateTime? exactDate = dateLostFound == null ? null : DateTime.ParseExact(dateLostFound, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            
            var searchPostId = this.searchPostService.Create(title, description, type, cityId, exactDate, petId, petName, imageUrl, speciesId, sizeId, ownerId, userId, phoneNumber);

            searchPostId.Should().NotBeEmpty();
            database.SearchPosts.Should().HaveCount(1);
        }

        [Fact]
        public void AllShouldReturnCorrectNumberOfSearchPosts()
        {
            this.mapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper();
            this.searchPostService = new SearchPostService(database, petService, mapper);
            database.SearchPosts.AddRange(GetLostSearchPosts());
            database.SaveChanges();


            var searchPosts = this.searchPostService.All(null, null, null, null, "Lost", 1, 10, Models.Shared.SearchPostSorting.DatePublished, "TestId");

            searchPosts.SearchPosts.Should().HaveCount(10);
        }

        [Theory]
        [InlineData("1", "New Title", "New Description", 2, null, "2")]
        [InlineData("1", "New Title", "New Description", 2, "15/05/2021", "2")]
        public void EditShouldWorkCorrectlyWhenSearchPostExists(string id, string title, string description, int cityId, string dateLostFound, string petId)
        {
            DateTime? exactDate = dateLostFound == null ? null : DateTime.ParseExact(dateLostFound, "dd/MM/yyyy",  CultureInfo.InvariantCulture);

            database.AddRange(GetFoundSearchPosts());
            database.SaveChanges();

            var isEditSuccessful = this.searchPostService.Edit(id, title, description, cityId, exactDate, petId);
            var searchPost = database.SearchPosts.FirstOrDefault(searchPost => searchPost.Id == id);

            isEditSuccessful.Should().BeTrue();
            searchPost.Should().Match<SearchPost>((x) =>
                x.Title == title &&
                x.Description == description &&
                x.CityId == cityId &&
                x.DateLostFound == exactDate &&
                x.PetId == petId
                );
        }

        [Theory]
        [InlineData("11", "New Title", "New Description", 2, null, "2")]
        [InlineData(null, "New Title", "New Description", 2, "15/05/2021", "2")]
        public void EditShouldreturnFalseWhenSearchPostDoesNotExist(string id, string title, string description, int cityId, string dateLostFound, string petId)
        {
            DateTime? exactDate = dateLostFound == null ? null : DateTime.ParseExact(dateLostFound, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            database.AddRange(GetFoundSearchPosts());
            database.SaveChanges();

            var isEditSuccessful = this.searchPostService.Edit(id, title, description, cityId, exactDate, petId);
            var searchPost = database.SearchPosts.FirstOrDefault(searchPost => searchPost.Id == id);

            isEditSuccessful.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test", "1")]
        public void DeleteShouldWorkWhenSearchPostExistsAnduserIsAuthorized(string userId, string searchPostId)
        {
            database.Users.Add(new IdentityUser { Id = userId });
            database.SearchPosts.Add(new SearchPost { Id = searchPostId, UserId = userId, SearchPostType = new SearchPostType { Id = 1, Name = "Found"} });

            database.SaveChanges();

            var isDeleteSuccessfull = this.searchPostService.Delete(searchPostId, userId, false);
            isDeleteSuccessfull.Item1.Should().BeTrue();
            var searchPostExists = database.SearchPosts.Any(searchPost => searchPost.Id == searchPostId);
            searchPostExists.Should().BeFalse();
        }

        [Theory]
        [InlineData("TestId", "TestId")]
        public void SaveWorksCorrectly(string userId, string searchPostId)
        {
            database.Users.Add(new IdentityUser { Id = userId });
            database.SearchPosts.Add(new SearchPost { Id = searchPostId, UserId = userId, SearchPostType = new SearchPostType { Id = 1, Name = "Found" } });

            database.SaveChanges();

            var isSaveSuccessfull = this.searchPostService.Save(searchPostId, userId);

            isSaveSuccessfull.Should().BeTrue();
            database.SavedSearchPosts.Any(saved => saved.SearchPostId == searchPostId && saved.UserId == userId);
        }

        [Theory]
        [InlineData("UserId", "1")]
        public void SavedShouldReturnCorrectNumberOfSearchPosts(string userId, string searchPostId)
        {
            this.mapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper();
            this.searchPostService = new SearchPostService(database, petService, mapper);
            this.database.SearchPosts.Add(new SearchPost { Id = searchPostId });
            this.database.SaveChanges();

            this.searchPostService.Save(searchPostId, userId);

            var searchPosts = this.searchPostService.Saved(userId);

            searchPosts.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("1", "Lost")]
        public void SetAsFoundClaimedWorksCorrectly(string searchPostId, string type)
        {
            this.database.SearchPosts.Add(new SearchPost { Id = searchPostId, 
                IsFoundClaimed = false, SearchPostType = new SearchPostType { Id = 1, Name = type} });
            this.database.SaveChanges();

            var searchPostType = this.searchPostService.SetAsFoundClaimed(searchPostId);

            searchPostType.Should().BeEquivalentTo(type);
            var isFoundClaimed = this.database.SearchPosts.FirstOrDefault(searchPost => searchPost.Id == searchPostId).IsFoundClaimed;

            isFoundClaimed.Should().BeTrue();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void CityExistsReturnsCorrectResult(int cityId)
        {
            this.database.AddRange(GetCities());
            this.database.SaveChanges();

            var cityExists = this.searchPostService.CityExists(cityId);

            cityExists.Should().BeTrue();

            var fakeCity = this.searchPostService.CityExists(20);
            fakeCity.Should().BeFalse();
        }

        [Theory]
        [InlineData("2")]
        [InlineData("4")]
        public void PetExistsReturnsCorrectResult(string petId)
        {
            this.database.AddRange(GetPets());
            this.database.SaveChanges();

            var cityExists = this.searchPostService.PetExists(petId);

            cityExists.Should().BeTrue();

            var fakeCity = this.searchPostService.PetExists("Test");
            fakeCity.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test", "1")]
        public void RemoveRemovesSavedSearchPost(string userId, string searchPostId)
        {
            this.database.SearchPosts.Add(new SearchPost { Id = searchPostId });
            this.database.SaveChanges();

            this.searchPostService.Save(searchPostId, userId);
            database.SavedSearchPosts.Any(saved => saved.UserId == userId && saved.SearchPostId == searchPostId)
                    .Should().BeTrue();
            this.searchPostService.Remove(searchPostId, userId);
            database.SavedSearchPosts.Any(saved => saved.UserId == userId && saved.SearchPostId == searchPostId)
        .Should().BeFalse();

        }


    }
}
