using FluentAssertions;
using MyTested.AspNetCore.Mvc;
using PetFinder.Controllers;
using PetFinder.Data.Models;
using PetFinder.Models.SearchPosts;
using System.Collections.Generic;
using Xunit;
using static PetFinder.Tests.Data.SearchPostsData;
using static PetFinder.Tests.Data.SizeData;
using static PetFinder.Tests.Data.SpeciesData;
using static PetFinder.Tests.Data.PetData;
using static PetFinder.Tests.Data.CitiesData;
using PetFinder.Services.SearchPosts.Models;
using PetFinder.Models.Pets;
using PetFinder.Models.Shared;

namespace PetFinder.Tests.Controllers
{
    public class SearchPostsControllerTest
    {
        [Fact]
        public void AllShouldReturnViewWithCorrectResult()
        {
            MyController<SearchPostsController>
                .Instance()
                .WithData(GetLostSearchPosts())
                .Calling(c => c.All(new AllSearchPostsViewModel { Type = "Lost", Pagination = new PaginationViewModel { PostsPerPage = 9} }, 1))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AllSearchPostsViewModel>()
                .Passing(m => m.SearchPosts.Should().HaveCount(9)));

            MyController<SearchPostsController>
                .Instance()
                .WithData(GetFoundSearchPosts())
                .Calling(c => c.All(new AllSearchPostsViewModel { Type = "Found", Pagination = new PaginationViewModel { PostsPerPage = 11} }, 1))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AllSearchPostsViewModel>()
                .Passing(m => m.SearchPosts.Should().HaveCount(10)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Test")]
        public void AllShouldReturnBadRequestIfSearchPostTypeIsInvalid(string type)
        {
            MyController<SearchPostsController>
                .Instance()
                .Calling(c => c.All(new AllSearchPostsViewModel { Type = type }, 1))
                .ShouldReturn()
                .BadRequest();

        }

        [Theory]
        [InlineData("Test", "Test", "Description", "1", 1)]
        public void DetailsShouldReturnViewWithCorrectResult(string id, string title, string description, string petId, int cityId)
        {
            MyController<SearchPostsController>
                .Instance()
                .WithUser()
                .WithData(new Owner { Id = 1, PhoneNumber = "0565548545", UserId = "TestId"}, GetSize(), GetSpecies() , GetPet(petId, "Test", 1, 2, "test", 1), GetCity(), GetSearchPost(id, title, description, petId, cityId, null))
                .Calling(c => c.Details(new SearchPostDetailsViewModel { Pagination = new PaginationViewModel { PostsPerPage = 9} }, "Test", 1))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<SearchPostDetailsViewModel>()
                .Passing(m => m.SearchPost.Title.Should().BeEquivalentTo("Test")));

            MyController<SearchPostsController>
                .Instance()
                .WithUser()
                .WithData(new Owner { Id = 1, PhoneNumber = "0565548545", UserId = "TestId" }, GetSize(), GetSpecies(), GetPet(petId, "Test", 1, 2, "test", 1), GetCity(), GetSearchPost(id, title, description, petId, cityId, null))
                .Calling(c => c.Details(new SearchPostDetailsViewModel { Pagination = new PaginationViewModel { PostsPerPage = 9 } }, "1", 1))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void MinePageShouldReturnViewWithCorrectResult()
        {
            MyController<SearchPostsController>
               .Instance()
               .WithUser()
               .WithData(GetSearchPost("1", "Test", "Description", null, 1, "TestId"))
               .Calling(c => c.Mine())
               .ShouldReturn()
               .View(view => view
               .WithModelOfType<IEnumerable<SearchPostServiceModel>>()
               .Passing(m => m.Should().HaveCount(1)));

            MyController<SearchPostsController>
               .Instance()
               .WithUser()
               .WithData(GetSearchPost("1", "Test", "Description", null, 1, "OtherId"))
               .Calling(c => c.Mine())
               .ShouldReturn()
               .View(view => view
               .WithModelOfType<IEnumerable<SearchPostServiceModel>>()
               .Passing(m => m.Should().HaveCount(0)));
        }

        [Fact]
        public void AddPageShouldReturnView()
        {
            MyController<SearchPostsController>
                .Instance()
                .Calling(c => c.Add("Found"))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AddSearchPostFormModel>());

            MyController<SearchPostsController>
                .Instance()
                .WithUser()
                .WithData(new Owner { UserId = "TestId"})
                .Calling(c => c.Add("Lost"))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AddSearchPostFormModel>());
        }

        [Fact]
        public void AddPageShouldRedirectToBecomeOwnerIfNonOwnerUserTriesToAccessAddLostSearchPostPage()
        {
            MyController<SearchPostsController>
                .Instance()
                .WithUser()
                .Calling(c => c.Add("Lost"))
                .ShouldReturn()
                .RedirectToAction("Become", "Owners");
        }

        [Theory]
        [InlineData("Test", "Description", "Found", "My Pet", "https://tinyurl.com/4fztbse4", 1, 2, 1, "085455245")]
        [InlineData("Test", "Description", "Lost", "My Pet", "https://tinyurl.com/4fztbse4", 1, 2, 1, "+359525878")]
        public void AddLogicShouldWorkCorrectlyWhenPassedDataIsValid(string title, string description, string type, string petName, string imageUrl, int sizeId, int speciesId, int cityId, string phoneNumber)
        {
            MyController<SearchPostsController>
                .Instance()
                .WithUser()
                .WithUser()
                .WithData(new Owner { UserId = "TestId" })
                .WithData(GetCity(), GetSize(), GetSpecies())
                .Calling(c => c.Add(new AddSearchPostFormModel { SearchPostType = type, PetId = "0", PhoneNumber = phoneNumber,  CityId = cityId, Title = title, Description = description, Pet = new AddPetFormModel { Name = petName, ImageUrl = imageUrl, SizeId = sizeId, SpeciesId = speciesId} }))
                .ShouldReturn()
                .RedirectToAction("All", new { Type = type });

            MyController<SearchPostsController>
                .Instance()
                .WithUser()
                .WithData(new Owner { UserId = "TestId"})
                .WithData(GetCity(), GetSize(), GetSpecies())
                .Calling(c => c.Add(new AddSearchPostFormModel { SearchPostType = type, PetId = "0", PhoneNumber = phoneNumber, CityId = cityId, Title = title, Description = description, Pet = new AddPetFormModel { Name = petName, ImageUrl = imageUrl, SizeId = sizeId, SpeciesId = speciesId } }))
                .ShouldReturn()
                .RedirectToAction("All", new { Type = type });
        }

        [Theory]
        [InlineData("Test", "Description", "Found", "My Pet", "https://tinyurl.com/4fztbse4", 2, 2, 1)]
        [InlineData("Test", "Description", "Found", "My Pet", "https://tinyurl.com/4fztbse4", 1, 3, 1)]
        [InlineData("Test", "Description", "Found", "My Pet", "https://tinyurl.com/4fztbse4", 1, 3, 10)]
        [InlineData("Test", "Description", "Found", "My Pet", null, 1, 3, 10)]
        public void AddLogicShouldReturnViewIfModelStateIsInvalid(string title, string description, string type, string petName, string imageUrl, int sizeId, int speciesId, int cityId)
        {
            MyController<SearchPostsController>
               .Instance()
               .WithUser()
               .WithData(GetCity(), GetSize(), GetSpecies())
               .Calling(c => c.Add(new AddSearchPostFormModel { SearchPostType = type, PetId = "0", CityId = cityId, Title = title, Description = description, Pet = new AddPetFormModel { Name = petName, ImageUrl = imageUrl, SizeId = sizeId, SpeciesId = speciesId } }))
               .ShouldReturn()
               .View(view => view
               .WithModelOfType<AddSearchPostFormModel>());
        }

        [Fact]
        public void EditShouldReturnView()
        {
            MyController<SearchPostsController>
                .Instance()
                .WithUser()
                .WithData(GetPet("Test", "Maxi", 1, 2, "image", 1), GetSearchPost("Test", "Title", "Description", "Test", 1, "TestId"))
                .Calling(c => c.Edit("Test"))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AddSearchPostFormModel>());
        }

        [Fact]
        public void SaveShouldReturnNotFoundIfSearchPostDoesNotExist()
        {
            MyController<SearchPostsController>
                .Instance()
                .WithUser()
                .Calling(c => c.Save("Test"))
                .ShouldReturn()
                .NotFound();
        }
    }
}
