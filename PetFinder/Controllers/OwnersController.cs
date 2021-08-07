using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Infrastructure;
using PetFinder.Models.Owners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetFinder.Data.Models;

namespace PetFinder.Controllers
{
    public class OwnersController : Controller
    {

        private readonly ApplicationDbContext context;

        public OwnersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        public IActionResult Become()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeOwnerFormModel owner)
        {
            var userId = this.User.GetId();

            var userIsAlreadyOwner = this.context.Owners.Any(owner => owner.UserId == userId);

            if(userIsAlreadyOwner)
            {
                return this.BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return this.View(owner);
            }

            var newOwner = new Owner
            {
                Name = owner.Name,
                PhoneNumber = owner.PhoneNumber,
                UserId = userId,
            };

            this.context.Owners.Add(newOwner);

            this.context.SaveChanges();

            return this.RedirectToAction("All", "SearchPosts");
        }
    }
}
