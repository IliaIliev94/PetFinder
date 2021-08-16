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
                .WithUser("Test", "Test", WebConstantscs.AdministratorRoleName)
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
                .WithUser("Test", "Test", WebConstantscs.AdministratorRoleName)
                .Calling(c => c.Add(new AddResourcePostFormModel { Title = title, Description = description, ImageUrl = imageUrl }))
                .ShouldReturn()
                .RedirectToAction("Details");
        }
    }
}
