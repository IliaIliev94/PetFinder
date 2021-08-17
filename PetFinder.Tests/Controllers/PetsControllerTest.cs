using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MyTested.AspNetCore.Mvc;
using PetFinder.Controllers;
using PetFinder.Data.Models;
using PetFinder.Models.Pets;
using PetFinder.Services.Pets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static PetFinder.Tests.Data.SizeData;
using static PetFinder.Tests.Data.SpeciesData;
using static PetFinder.Tests.Data.PetData;

namespace PetFinder.Tests.Controllers
{
    public class PetsControllerTest
    {
        [Fact]
        public void AllMethodShouldReturnCorrectViewModel()
        {
            MyController<PetsController>
                .Instance()
                .WithUser()
                .WithData(new Owner { Id = 1, Name = "Test", PhoneNumber = "089854245", UserId = "TestId"})
                .Calling(c => c.All())
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<IEnumerable<PetServiceModel>>());
        }

        [Fact]
        public void AdminShouldBeAbleToViewAllPets()
        {
            MyController<PetsController>
                .Instance()
                .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
                .Calling(c => c.All())
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<IEnumerable<PetServiceModel>>());
        }

        [Fact]
        public void NonOwnerShouldGetUnauthorized()
        {
            MyController<PetsController>
                .Instance()
                .WithUser()
                .Calling(c => c.All())
                .ShouldReturn()
                .Unauthorized();
        }

        [Fact]
        public void OnlyOwnersShouldBeAllowedToAccessAddPet()
        {
            MyController<PetsController>
                .Instance()
                .WithUser()
                .WithData(new Owner { UserId = "TestId" })
                .Calling(c => c.Add(null, 0))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AddPetFormModel>());

            MyController<PetsController>
                .Instance()
                .WithUser()
                .Calling(c => c.Add(null, 0))
                .ShouldReturn()
                .RedirectToAction("Become", "Owners");
        }

        [Theory]
        [InlineData("test", "test", 1, 2, "test", 2)]
        [InlineData("2", "Maxi", 1, 2, "imageUrl", 2)]
        public void OwnersAndAdminsShouldBeAllowedToAccessDetailsPage(string petId, string name, int sizeId, int speciesId, string imageUrl, int ownerId)
        {
            MyController<PetsController>
               .Instance()
               .WithUser()
               .WithData(new Owner { Id = ownerId, UserId = "TestId" },
               GetSize(),
               GetSpecies(),
               GetPet(petId, name, sizeId, speciesId, imageUrl, ownerId))
               .Calling(c => c.Details(petId))
               .ShouldReturn()
               .View(view => view
               .WithModelOfType<PetDetailsServiceModel>());

            MyController<PetsController>
              .Instance()
              .WithUser("Test", "Test", WebConstants.AdministratorRoleName)
              .WithData(
              GetSize(),
              GetSpecies(),
              GetPet(petId, name, sizeId, speciesId, imageUrl, ownerId))
              .Calling(c => c.Details(petId))
              .ShouldReturn()
              .View(view => view
              .WithModelOfType<PetDetailsServiceModel>());
        }

        [Theory]
        [InlineData("test", "test", 1, 2, "test", 2)]
        public void RegularUsersShouldRecieveUnauthorizedWhenAccessingDetailsPage(string petId, string name, int sizeId, int speciesId, string imageUrl, int ownerId)
        {
            MyController<PetsController>
                .Instance()
                .WithData(GetSize(), GetSpecies(),
                GetPet(petId, name, sizeId, speciesId, imageUrl, ownerId))
                .Calling(c => c.Details(petId))
                .ShouldReturn()
                .Unauthorized();
        }

        [Fact]
        public void RegularUsersShouldRecieveUnauthorizedWhenAccessingAddPage()
        {
            MyController<PetsController>
                .Instance()
                .Calling(c => c.Add("", 1))
                .ShouldReturn()
                .RedirectToAction("Become", "Owners");
        }

        [Fact]
        public void AddPetShouldReturnCorrectViewModel()
        {
            MyController<PetsController>
                .Instance()
                .WithUser()
                .WithData(new Owner { UserId = "TestId" })
                .Calling(c => c.Add("", 1))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AddPetFormModel>());
        }

        [Theory]
        [InlineData("Test", "image", 1, 2)]
        [InlineData("Maxi", "sss", 1, 2)]
        public void AddPetShouldWorkCorrectlyWhenPassedCorrectData(string petName, string imageUrl, int sizeId, int speciesId)
        {
            MyController<PetsController>
                .Instance()
                .WithUser()
                .WithData(new Owner { Id = 1, UserId = "TestId" },
                GetSize(),
                GetSpecies())
                .Calling(c => c.Add(new AddPetFormModel
                { Name = petName, ImageUrl = imageUrl, SizeId = sizeId, SpeciesId = speciesId }))
                .ShouldReturn()
                .RedirectToAction("Details");
        }

        [Fact]
        public void AddPetShouldRedirectToItselfWhenModelStateIsInvalid()
        {
            MyController<PetsController>
               .Instance()
               .WithUser()
               .WithData(new Owner { Id = 1, UserId = "TestId" },
               GetSize(),
               GetSpecies())
               .Calling(c => c.Add(new AddPetFormModel
               { Name = "Maxi", ImageUrl = "", SizeId = 4, SpeciesId = 5 }))
               .ShouldReturn()
               .View(view => view
               .WithModelOfType<AddPetFormModel>());
        }
    }
}
