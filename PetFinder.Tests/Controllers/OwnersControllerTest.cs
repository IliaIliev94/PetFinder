using MyTested.AspNetCore.Mvc;
using PetFinder.Controllers;
using PetFinder.Data.Models;
using PetFinder.Models.Owners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetFinder.Tests.Controllers
{
    public class OwnersControllerTest
    {
        [Fact]
        public void BecomeShouldReturnView()
        {
            MyController<OwnersController>
                .Instance()
                .Calling(c => c.Become())
                .ShouldReturn()
                .View();
        }

        [Theory]
        [InlineData("Test", "+3594512548")]
        [InlineData("Viktor", "089855785")]
        public void BecomeShouldRedirectoToHomeIndexViewOnSuccess(string name, string phoneNumber)
        {
            MyController<OwnersController>
                .Instance()
                .Calling(c => c.Become(new BecomeOwnerFormModel { Name = name, PhoneNumber = phoneNumber }))
                .ShouldReturn()
                .RedirectToAction("Index", "Home");
        }

        [Fact]
        public void BecomeShouldRedirectToItselfWhenModelStateIsInvalid()
        {
            MyController<OwnersController>
                .Instance()
                .Calling(c => c.Become(new BecomeOwnerFormModel { Name = "", PhoneNumber = "Hello" }))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<BecomeOwnerFormModel>());
        }

        [Fact]
        public void BecomeShouldReturnBadRequestWhenOwnerTriesToBecomeOwnerAgain()
        {
            MyController<OwnersController>
                .Instance()
                .WithUser()
                .WithData(new Owner { UserId = "TestId" })
                .Calling(c => c.Become(new BecomeOwnerFormModel { Name = "Viktor", PhoneNumber = "08985854" }))
                .ShouldReturn()
                .BadRequest();
        }


    }
}
