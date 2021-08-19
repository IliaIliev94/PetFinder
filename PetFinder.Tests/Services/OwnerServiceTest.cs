using FluentAssertions;
using PetFinder.Data.Models;
using PetFinder.Services.Owners;
using PetFinder.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetFinder.Tests.Services
{
    public class OwnerServiceTest
    {
        private IOwnerService ownerService;
        [Theory]
        [InlineData("TestId")]
        [InlineData("NewId")]
        public void IsOwnerShouldReturnCorrectResult(string userId)
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            this.ownerService = new OwnerService(database, mapper);

            database.Owners.Add(new Owner { Id = 1, Name = "Test", PhoneNumber = "08978548", UserId = userId });
            database.SaveChanges();

            this.ownerService.IsOwner(userId).Should().BeTrue();

            this.ownerService.IsOwner("User").Should().BeFalse();
        }

        [Theory]
        [InlineData(1, "TestId")]
        [InlineData(4 ,"NewId")]
        public void GetOwnerIdShouldReturnCorrectResult(int ownerId, string userId)
        {
            var database = DatabaseMock.Instance;
            var mapper = MapperMock.Mapper;
            this.ownerService = new OwnerService(database, mapper);

            database.Owners.Add(new Owner { Id = ownerId, Name = "Test", PhoneNumber = "08978548", UserId = userId });
            database.SaveChanges();

            var result = this.ownerService.GetOwnerId(userId);
            Assert.Equal(ownerId, result);
        }


    }
}
