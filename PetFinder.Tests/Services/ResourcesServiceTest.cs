using AutoMapper;
using FluentAssertions;
using PetFinder.Data;
using PetFinder.Data.Models;
using PetFinder.Infrastructure;
using PetFinder.Services.Resources;
using PetFinder.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetFinder.Tests.Services
{
    public class ResourcesServiceTest
    {
        private IResourcesService resourcesService;
        private readonly ApplicationDbContext database;
        private IMapper mapper;

        public ResourcesServiceTest()
        {
            database = DatabaseMock.Instance;
            mapper = MapperMock.Mapper;
            this.resourcesService = new ResourcesService(database, mapper);
        }

        [Theory]
        [InlineData("Title", "Description", "ImageUrl")]
        [InlineData("Test", "Test", "Test")]
        public void CreateShouldWorkCorrectly(string title, string description, string imageUrl)
        {
            this.resourcesService = new ResourcesService(database, mapper);
            database.ResourcePosts.Should().HaveCount(0);
            this.resourcesService.Create(title, description, imageUrl);

            database.ResourcePosts.Should().HaveCount(1);

            var resourcePost = database.ResourcePosts.FirstOrDefault(resourcePost => resourcePost.Title == title);

            resourcePost.Should().Match<ResourcePost>((x) =>
              x.Title == title &&
              x.ImageUrl == imageUrl &&
              x.Description == description
              );
        }

        [Theory]
        [InlineData("Title", "Description", "ImageUrl")]
        public void AllShouldReturnCorrectResult(string title, string description, string imageUrl)
        {
            this.mapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper();

            this.resourcesService = new ResourcesService(database, mapper);

            for(int i = 0; i < 10; i++)
            {
                this.resourcesService.Create(title, description, imageUrl);
            }

            var resourcePosts = this.resourcesService.All();

            resourcePosts.Should().HaveCount(10);
        }

        [Theory]
        [InlineData("Title", "Description", "ImageUrl")]
        [InlineData("Test", "Test", "Test")]
        public void DetailsShouldReturnCorrectResult(string title, string description, string imageUrl)
        {
            this.mapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper();

            this.resourcesService = new ResourcesService(database, mapper);

            var resourcePostId = this.resourcesService.Create(title, description, imageUrl);

            var resourcePost = this.resourcesService.Details(resourcePostId);

            resourcePost.Id.Should().BeEquivalentTo(resourcePostId);
        }

        [Theory]
        [InlineData("Title", "Description", "ImageUrl")]
        [InlineData("New Title", "New Description", "https://tinyurl.com/4fztbse4")]
        public void EditShouldWorkCorrectly(string title, string description, string imageUrl)
        {
            var resourcePostId = this.resourcesService.Create("Test Title", "Test Description", "Test Image Url");
            var isEditSuccessfull = this.resourcesService.Edit(resourcePostId, title, description, imageUrl);

            isEditSuccessfull.Should().BeTrue();

            var resourcePost = database.ResourcePosts.FirstOrDefault(resourcePost => resourcePost.Id == resourcePostId);

            resourcePost.Should().Match<ResourcePost>((x) =>
              x.Title == title &&
              x.ImageUrl == imageUrl &&
              x.Description == description
              );
        }

        [Fact]
        public void EditShouldReturnFalseIfNonExistingResourcePostIsSelected()
        {
            var resourcePostId = this.resourcesService.Create("Test Title", "Test Description", "Test Image Url");
            var isEditSuccessfull = this.resourcesService.Edit("TestId", "New Title", "New Description", "New Image Url");

            isEditSuccessfull.Should().BeFalse();
        }

        [Theory]
        [InlineData("Title", "Description", "ImageUrl")]
        [InlineData("New Title", "New Description", "https://tinyurl.com/4fztbse4")]
        public void DeleteShouldWorkCorrectly(string title, string description, string imageUrl)
        {
            database.ResourcePosts.Should().HaveCount(0);
            var resourcePostId = this.resourcesService.Create(title, description, imageUrl);
            database.ResourcePosts.Should().HaveCount(1);

            var isDeleteSuccessfull = this.resourcesService.Delete(resourcePostId);
            isDeleteSuccessfull.Should().BeTrue();
            database.ResourcePosts.Should().HaveCount(0);
        }

        [Fact]
        public void DeleteShouldReturnFalseIfNoneExistingResourcePostIsSelected()
        {
            var isDeleteSuccessfull = this.resourcesService.Delete("Test");

            isDeleteSuccessfull.Should().BeFalse();
        }
    }
}
