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
using PetFinder.Services.Owners;
using AutoMapper;
using PetFinder.Common.Messages;

namespace PetFinder.Controllers
{
    public class OwnersController : Controller
    {

        private readonly IOwnerService ownersService;
        private readonly IMapper mapper;

        public OwnersController(IOwnerService ownersService, IMapper mapper)
        {
            this.ownersService = ownersService;
            this.mapper = mapper;
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

            var userIsAlreadyOwner = this.ownersService.IsOwner(userId);

            if(userIsAlreadyOwner)
            {
                return this.BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return this.View(owner);
            }


            this.ownersService.Add(owner.Name, owner.PhoneNumber, this.User.GetId());

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Edit()
        {
            var ownerData = this.mapper.Map<BecomeOwnerFormModel>(this.ownersService.GetOwnerData(this.User.GetId()));
            return this.View(ownerData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(BecomeOwnerFormModel owner)
        {
            var userId = this.User.GetId();
            if (!this.ownersService.IsOwner(userId))
            {
                return this.Unauthorized();
            }

            if(!ModelState.IsValid)
            {
                var ownerData = this.mapper.Map<BecomeOwnerFormModel>(this.ownersService.GetOwnerData(this.User.GetId()));
                return this.View(ownerData);
            }

            var ownerId = this.ownersService.GetOwnerId(this.User.GetId());

            this.ownersService.Edit(userId, owner.Name, owner.PhoneNumber);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
