using AutoMapper;
using FluentAssertions;
using PetFinder.Data;
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
        private readonly ApplicationDbContext database = DatabaseMock.Instance;
        private readonly IMapper mapper = MapperMock.Mapper;

        public OwnerServiceTest()
        {
            this.ownerService = new OwnerService(database, mapper);
        }

        [Theory]
        [InlineData("TestId")]
        [InlineData("NewId")]
        public void IsOwnerShouldReturnCorrectResult(string userId)
        {
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
            database.Owners.Add(new Owner { Id = ownerId, Name = "Test", PhoneNumber = "08978548", UserId = userId });
            database.SaveChanges();

            var result = this.ownerService.GetOwnerId(userId);
            Assert.Equal(ownerId, result);
        }
        [Theory]
        [InlineData("TestUser", "08985455465", "TestId")]
        public void AddWorksCorrectly(string name, string phoneNumber, string userId)
        {

            this.ownerService.Add(name, phoneNumber, userId);

            database.Owners.Where(owner => owner.Name == name
                && owner.PhoneNumber == phoneNumber
                && owner.UserId == userId).Should().HaveCount(1);

        }

        [Theory]
        [InlineData("TestUser", "08985455465", "TestId")]
        public void GetPhoneNumberShouldReturnCorrectResult(string name, string phoneNumber, string userId)
        {

            this.ownerService.Add(name, phoneNumber, userId);

            var phoneNumberResult = this.ownerService.GetPhoneNumber(userId);
            phoneNumberResult.Should().BeEquivalentTo(phoneNumber);
        }

        [Theory]
        [InlineData("TestUser", "08985455465", "TestId")]
        public void EditShouldWorkCorrectly(string name, string phoneNumber, string userId)
        {

            this.ownerService.Add("Initial name", "Initial phone number", userId);

            this.ownerService.Edit(userId, name, phoneNumber);

            var owner = database.Owners.FirstOrDefault(owner => owner.UserId == userId);
            owner.Should().Match<Owner>((x) =>
                x.Name == name &&
                x.PhoneNumber == phoneNumber
                );
        }

        [Theory]
        [InlineData("Test", "Test", "TestId")]
        public void GetEditDataShouldReturnCorrectResult(string name, string phoneNumber, string userId)
        {
            this.ownerService.GetOwnerData(userId);
        }
    }
}
