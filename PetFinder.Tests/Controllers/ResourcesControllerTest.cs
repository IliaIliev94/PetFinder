using FluentAssertions;
using MyTested.AspNetCore.Mvc;
using PetFinder.Controllers;
using PetFinder.Models.Resources;
using PetFinder.Services.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using static PetFinder.Tests.Data.ResourcesData;

namespace PetFinder.Tests.Controllers
{
    public class ResourcesControllerTest
    {
        [Fact]
        public void AllShouldReturnView()
        {
            MyController<ResourcesController>
                .Instance()
                .WithData(GetResourcePosts())
                .Calling(c => c.All())
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<IEnumerable<ResourcePostServiceModel>>()
                .Passing(m => m.Should().HaveCount(10)));
        }

        [Fact]
        public void AddShouldOnlyBeAccessibleByAdmins()
        {
            MyController<ResourcesController>
                .Instance()
                .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
                .Calling(c => c.Add())
                .ShouldReturn()
                .View();
        }

        [Theory]
        [InlineData("Test", "Test", "Test")]
        public void OnlyAdminShouldBeAbleToAddResourcePost(string title, string description, string imageUrl)
        {
            MyController<ResourcesController>
                .Instance()
                .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
                .Calling(c => c.Add(new AddResourcePostFormModel { Title = title, Description = description, ImageUrl = imageUrl }))
                .ShouldReturn()
                .RedirectToAction("Details");

            MyController<ResourcesController>
                .Instance()
                .WithUser()
                .Calling(c => c.Add(new AddResourcePostFormModel { Title = title, Description = description, ImageUrl = imageUrl }))
                .ShouldReturn()
                .Unauthorized();
        }

        [Fact]
        public void DetailsShouldReturnView()
        {
            MyController<ResourcesController>
                .Instance()
                .WithData(GetResourcePosts())
                .Calling(c => c.Details("0"))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<ResourcePostDetailsServiceModel>()
                .Passing(m => m.Id.Should().BeEquivalentTo("0")));
        }

        [Fact]
        public void EditShouldReturnView()
        {
            MyController<ResourcesController>
                .Instance()
                .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
                .WithData(GetResourcePosts())
                .Calling(c => c.Edit("0"))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AddResourcePostFormModel>()
                .Passing(m => m.Title.Should().BeEquivalentTo("0")));

            MyController<ResourcesController>
               .Instance()
               .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
               .WithData(GetResourcePosts())
               .Calling(c => c.Edit("11"))
               .ShouldReturn()
               .NotFound();
        }

        [Theory]
        [InlineData("1", "New", "New Description", "Image url")]
        [InlineData("3", "Test", "Description", "https://tinyurl.com/4fztbse4")]
        public void OnlyAdminShouldBeAbleToEdit(string id, string title, string description, string imageUrl)
        {
            MyController<ResourcesController>
                .Instance()
                .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
                .WithData(GetResourcePosts())
                .Calling(c => c.Edit(new AddResourcePostFormModel {Id = id, Title = title, Description = description, ImageUrl = imageUrl }))
                .ShouldReturn()
                .RedirectToAction("Details", new { Id = id });

            MyController<ResourcesController>
               .Instance()
               .WithUser()
               .WithData(GetResourcePosts())
               .Calling(c => c.Edit(new AddResourcePostFormModel { Id = id, Title = title, Description = description, ImageUrl = imageUrl }))
               .ShouldReturn()
               .Unauthorized();
        }

        [Fact]
        public void EditShouldReturnNotFoundIfResourcePostDoesNotExist()
        {
            MyController<ResourcesController>
               .Instance()
               .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
               .WithData(GetResourcePosts())
               .Calling(c => c.Edit(new AddResourcePostFormModel { Id = "11", Title = "Test", Description = "Description", ImageUrl = "https://tinyurl.com/4fztbse4" }))
               .ShouldReturn()
               .NotFound();
        }

        [Theory]
        [InlineData("1", "New", "New", null)]
        [InlineData("2", null, "New", "https://tinyurl.com/4fztbse4")]
        [InlineData("3", "New", "", "https://tinyurl.com/4fztbse4")]
        public void EditShouldReturnViewIfDataIsInvalid(string id, string title, string description, string imageUrl)
        {
            MyController<ResourcesController>
                .Instance()
                .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
                .WithData(GetResourcePosts())
                .Calling(c => c.Edit(new AddResourcePostFormModel { Id = id, Title = title, Description = description, ImageUrl = imageUrl }))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AddResourcePostFormModel>()
                .Passing(m => m.Title.Should().BeEquivalentTo(title)));
        }

        [Fact]
        public void OnlyAdministratorShouldBeAbloToDeleteResourcePost()
        {

            MyController<ResourcesController>
               .Instance()
               .WithData(GetResourcePosts())
               .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
               .Calling(c => c.Delete("1"))
               .ShouldReturn()
               .RedirectToAction("All");

            MyController<ResourcesController>
                .Instance()
                .WithData(GetResourcePosts())
                .WithUser()
                .Calling(c => c.Delete("1"))
                .ShouldReturn()
                .Unauthorized();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("11")]
        [InlineData("105")]
        public void DeleteShouldReturnIfAnAttemptIsMadeToDeleteANonExistingPage(string id)
        {
            MyController<ResourcesController>
              .Instance()
              .WithData(GetResourcePosts())
              .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
              .Calling(c => c.Delete(id))
              .ShouldReturn()
              .NotFound();
        }
    }
}
