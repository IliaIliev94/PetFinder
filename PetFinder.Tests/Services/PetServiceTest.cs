using AutoMapper;
using FluentAssertions;
using PetFinder.Data.Models;
using PetFinder.Infrastructure;
using PetFinder.Services.Pets;
using PetFinder.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static PetFinder.Tests.Data.SizeData;
using static PetFinder.Tests.Data.SpeciesData;

namespace PetFinder.Tests.Services
{
    public class PetServiceTest
    {
        private IPetService petService;
        [Theory]
        [InlineData("Test", "ImageUrl", 1, 1, 2)]
        public void CreateShouldWorkWhenDataIsPassedCorrectly(string name, string imageUrl, int speciesId, int sizeId, int? ownerId)
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            this.petService = new PetService(database, mapper);

            var petId = this.petService.Create(name, imageUrl, speciesId, sizeId, ownerId);
            petId.Should().NotBe(null);

            database.Pets.Should().HaveCount(1);

            petService.Create(name, imageUrl, speciesId, sizeId, ownerId);

            database.Pets.Should().HaveCount(2);
        }

        [Theory]
        [InlineData("Test", "imageUrl", 2, 1, 2)]
        [InlineData("Maxi", "asd", 2, 1, 1)]
        public void DetailsShouldWorkCorrectly(string name, string imageUrl, int speciesId, int sizeId, int? ownerId)
        {
            var database = DatabaseMock.Instance;
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper();
            this.petService = new PetService(database, mapper);
            database.Sizes.AddRange(GetSize());
            database.Species.AddRange(GetSpecies());
            var petId = this.petService.Create(name, imageUrl, speciesId, sizeId, ownerId);

            var pet = this.petService.Details(petId);
            pet.Id.Should().BeEquivalentTo(petId);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetOwnerIdWorksCorrectlyWhenPassedValidData(int ownerId)
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            this.petService = new PetService(database, mapper);

            database.Owners.Add(new Owner { Id = ownerId, Name = "Test", PhoneNumber = "08978548", UserId = "2" });
            database.SaveChanges();

            var petId = this.petService.Create("Test", "Test", 1, 2, ownerId);

            var test = this.petService.GetOwnerId(petId);
            Assert.Equal(test, ownerId);
        }

        [Theory]
        [InlineData("Test", "ImageUrl", 1, 1, 2)]
        public void PetCountShouldReturnAccurateResult(string name, string imageUrl, int speciesId, int sizeId, int? ownerId)
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            this.petService = new PetService(database, mapper);

            database.Pets.Should().HaveCount(0);
            this.petService.Create(name, imageUrl, speciesId, sizeId, ownerId);
            this.petService.Create(name, imageUrl, speciesId, sizeId, ownerId);
            this.petService.Create(name, imageUrl, speciesId, sizeId, ownerId);
            var petsCount = this.petService.PetsCount();
            Assert.Equal(3, petsCount);
        }

        [Theory]
        [InlineData("New name", "New image url", 2, 3)]
        [InlineData("Maxi", "https://tinyurl.com/4fztbse4", 2, 3)]
        public void EditShouldWorkCorrectly(string name, string imageUrl, int speciesId, int sizeId)
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            this.petService = new PetService(database, mapper);

            var petId = this.petService.Create("Test", "ImageUrl", 1, 1, 2);

            var isEditsuccessfull = this.petService.Edit(petId, name, imageUrl, speciesId, sizeId);
            isEditsuccessfull.Should().BeTrue();
            var pet = database.Pets.FirstOrDefault(pet => pet.Id == petId);

            pet.Should().Match<Pet>((x) =>
               x.Name == name &&
               x.ImageUrl == imageUrl &&
               x.SpeciesId == speciesId &&
               x.SizeId == sizeId
               );
        }

        [Fact]
        public void DeleteShouldWorkCorrectly()
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            this.petService = new PetService(database, mapper);

            database.Pets.Should().HaveCount(0);
            var petId = this.petService.Create("Test", "ImageUrl", 1, 1, 2);
            database.Pets.Should().HaveCount(1);
            var isDeleteSuccessfull = this.petService.Delete(petId);

            isDeleteSuccessfull.Should().BeTrue();
            database.Pets.Should().HaveCount(0);
        }

        [Fact]
        public void DeleteShouldReturnFalseIfAnAttemptIsMadeToDeleteANonExistingPet()
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            this.petService = new PetService(database, mapper);

            var petId = this.petService.Create("Test", "ImageUrl", 1, 1, 2);
            var isDeleteSuccessfull = this.petService.Delete("Test");
            isDeleteSuccessfull.Should().BeFalse();
        }

        [Theory]
        [InlineData("Maxi", "https://tinyurl.com/4fztbse4", 2, 1, 1)]
        public void AllShouldReturnAllPetsOfTheGivenOwner(string name, string imageUrl, int speciesId, int sizeId, int ownerId)
        {
            var database = DatabaseMock.Instance;
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper();
            this.petService = new PetService(database, mapper);

            database.Owners.Add(new Owner { Id = ownerId, Name = "Test", PhoneNumber = "08978548", UserId = "2" });
            database.Sizes.AddRange(GetSize());
            database.Species.AddRange(GetSpecies());
            database.SaveChanges();

            for(int i = 0; i < 10; i++)
            {
                this.petService.Create(name, imageUrl, speciesId, sizeId, ownerId);
            }
            

            var pets = this.petService.All(ownerId);
            pets.Should().HaveCount(10);
        }
    }
}
