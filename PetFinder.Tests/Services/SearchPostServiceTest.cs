using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using PetFinder.Data.Models;
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

namespace PetFinder.Tests.Services
{
    public class SearchPostServiceTest
    {
        private SearchPostService searchPostService;

        [Theory]
        [InlineData("Test", "Description", "Found", 1, null, "2", "Jessy", "imageUrl", 1, 2, 1, "test", "1234")]
        [InlineData("12", "Some text", "Lost", 1, "12/05/2021", "2", "Jessy", "imageUrl", 1, 2, 1, "test", "089844524")]
        public void CreateShouldWorkWhenDataIsPassedCorrectly(string title, string description, string type, int cityId, string dateLostFound,
            string petId, string petName, string imageUrl, int speciesId, int sizeId, int? ownerId, string userId, string phoneNumber)
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            var petService = new PetService(database, mapper);
            DateTime? exactDate = dateLostFound == null ? null : DateTime.ParseExact(dateLostFound, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            this.searchPostService = new SearchPostService(database, petService, mapper);
            var searchPostId = this.searchPostService.Create(title, description, type, cityId, exactDate, petId, petName, imageUrl, speciesId, sizeId, ownerId, userId, phoneNumber);

            searchPostId.Should().NotBeEmpty();
            database.SearchPosts.Should().HaveCount(1);
        }

        [Fact]
        public void AllShouldReturnCorrectNumberOfSearchPosts()
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            var petService = new PetService(database, mapper);
            this.searchPostService = new SearchPostService(database, petService, mapper);
            database.SearchPosts.AddRange(GetLostSearchPosts());
            database.SaveChanges();

            var searchPosts = this.searchPostService.All(null, null, null, null, "Lost", 1, 10, Models.Shared.SearchPostSorting.DatePublished);

            searchPosts.SearchPosts.Should().HaveCount(10);
        }

        [Theory]
        [InlineData("1", "New Title", "New Description", 2, null, "2")]
        [InlineData("1", "New Title", "New Description", 2, "15/05/2021", "2")]
        public void EditShouldWorkCorrectlyWhenSearchPostExists(string id, string title, string description, int cityId, string dateLostFound, string petId)
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            var petService = new PetService(database, mapper);
            this.searchPostService = new SearchPostService(database, petService, mapper);
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
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            var petService = new PetService(database, mapper);
            this.searchPostService = new SearchPostService(database, petService, mapper);
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
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            var petService = new PetService(database, mapper);
            this.searchPostService = new SearchPostService(database, petService, mapper);

            database.Users.Add(new IdentityUser { Id = userId });
            database.SearchPosts.Add(new SearchPost { Id = searchPostId, UserId = userId, SearchPostType = new SearchPostType { Id = 1, Name = "Found"} });

            database.SaveChanges();

            var isDeleteSuccessfull = this.searchPostService.Delete(searchPostId, userId, false);
            isDeleteSuccessfull.Item1.Should().BeTrue();
            var searchPostExists = database.SearchPosts.Any(searchPost => searchPost.Id == searchPostId);
            searchPostExists.Should().BeFalse();
        }

       
    }
}
