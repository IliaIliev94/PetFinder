using AutoMapper;
using FluentAssertions;
using PetFinder.Data;
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
        private readonly ApplicationDbContext database;
        private IMapper mapper;

        public PetServiceTest()
        {
            this.database = DatabaseMock.Instance;
            this.mapper = MapperMock.Mapper;
            this.petService = new PetService(database, mapper);
        }

        [Theory]
        [InlineData("Test", "ImageUrl", 1, 1, 2)]
        public void CreateShouldWorkWhenDataIsPassedCorrectly(string name, string imageUrl, int speciesId, int sizeId, int? ownerId)
        {
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
            database.Pets.Should().HaveCount(0);
            var petId = this.petService.Create("Test", "ImageUrl", 1, 1, 2);
            database.Pets.Should().HaveCount(1);
            var isDeleteSuccessfull = this.petService.Delete(petId);

            isDeleteSuccessfull.Should().BeTrue();
            database.Pets.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("TestId", 1, "Maxi", "https://tinyurl.com/4fztbse4", 1, 2)]
        public void DeleteShouldWorkCorrectlyForOwners(string userId, int ownerId, string petName, string imageUrl, int speciesId, int sizeId)
        {
            this.database.Owners.Add(new Owner { Id = ownerId, Name = "Test", UserId = userId });
            this.database.SaveChanges();

            database.Pets.Should().HaveCount(0);
            var petId = this.petService.Create(petName, imageUrl, speciesId, sizeId, ownerId);
            database.Pets.Should().HaveCount(1);

            var isDeleteSuccessfull = this.petService.Delete(petId, ownerId);
            isDeleteSuccessfull.Should().BeTrue();
            database.Pets.Should().HaveCount(0);
        }

        [Fact]
        public void DeleteShouldReturnFalseIfAnAttemptIsMadeToDeleteANonExistingPet()
        {
            var petId = this.petService.Create("Test", "ImageUrl", 1, 1, 2);
            var isDeleteSuccessfull = this.petService.Delete("Test");
            isDeleteSuccessfull.Should().BeFalse();
        }

        [Fact]
        public void DeleteShouldReturnFalseIfUserDoesNotOwnPet()
        {
            var petId = this.petService.Create("Test", "ImageUrl", 1, 1, 2);
            var isDeleteSuccessfull = this.petService.Delete(petId, 3);
            isDeleteSuccessfull.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, "Maxi", "https://tinyurl.com/4fztbse4", 2, 4, "TestSearchPost")]
        public void DeleteShouldReturnFalseIfPetBelongsToASearchPost(int ownerId, string petName, 
            string imageUrl, int speciesId, int sizeId, string searchPostId)
        {
            var petId = this.petService.Create(petName, imageUrl, speciesId, sizeId, ownerId);
            this.database.SearchPosts.Add(new SearchPost { Id = searchPostId, PetId = petId });
            this.database.SaveChanges();

            var isDeleteSuccessfull = this.petService.Delete(petId, ownerId);

            isDeleteSuccessfull.Should().BeFalse();

        }
        
        [Theory]
        [InlineData("Maxi", "https://tinyurl.com/4fztbse4", 2, 1, 1)]
        public void AllShouldReturnAllPetsOfTheGivenOwner(string name, string imageUrl, int speciesId, int sizeId, int ownerId)
        {
            this.mapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper();
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

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(10)]
        public void PetSpeciesExistsReturnsCorrectResult(int id)
        {
            this.database.Species.AddRange(Enumerable.Range(1, 10).Select(i => new Specie { Id = i, Name = i.ToString() }));
            this.database.SaveChanges();

            var speciesExists = this.petService.SpeciesExists(id);
            speciesExists.Should().BeTrue();

            speciesExists = this.petService.SpeciesExists(20);
            speciesExists.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(10)]
        public void PetSizeExistsReturnsCorrectResult(int id)
        {
            this.database.Sizes.AddRange(Enumerable.Range(1, 10).Select(i => new Size { Id = i, Type = i.ToString() }));
            this.database.SaveChanges();

            var speciesExists = this.petService.SizeExists(id);
            speciesExists.Should().BeTrue();

            speciesExists = this.petService.SizeExists(20);
            speciesExists.Should().BeFalse();
        }
    }
}
