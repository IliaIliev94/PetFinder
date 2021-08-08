using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Models.Pets;
using PetFinder.Services.Pets;
using PetFinder.Services.Owners;
using PetFinder.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace PetFinder.Controllers
{
    public class PetsController : Controller
    {
        private readonly IPetService petService;
        private readonly IOwnerService ownerService;

        public PetsController(IPetService petService, IOwnerService ownerService)
        {
            this.petService = petService;
            this.ownerService = ownerService;
        }

        [Authorize]
        public IActionResult All()
        {

            if(!this.ownerService.IsOwner(this.User.GetId()))
            {
                return this.BadRequest();
            }

            var pets = this.petService.All();

            return this.View(pets);
        }

        [Authorize]
        public IActionResult Details(string id)
        {

            if (!this.ownerService.IsOwner(this.User.GetId()))
            {
                return this.BadRequest();
            }

            var pet = this.petService.Details(id);

            if(pet == null)
            {
                return this.BadRequest();
            }

            return this.View(pet);

        }

        [Authorize]
        public IActionResult Add(string searchId, int ownerId)
        {

            if (!this.ownerService.IsOwner(this.User.GetId()))
            {
                return this.BadRequest();
            }

            ViewBag.SearchId = searchId;
            ViewBag.OwnerId = ownerId;
           
            return this.View(new AddPetFormModel 
            { 
                Sizes = this.petService.GetSizes(),
                Species = this.petService.GetSpecies(),
            });
        }

        [HttpPost]
        public IActionResult Add(AddPetFormModel pet)
        {

            if (!this.ownerService.IsOwner(this.User.GetId()))
            {
                return this.BadRequest();
            }

            if (!ModelState.IsValid)
            {
                pet.Sizes = this.petService.GetSizes();
                pet.Species = this.petService.GetSpecies();

                return this.View(pet);
            }


            var newPet = this.petService.Create(pet.Name, pet.ImageUrl, pet.SpeciesId, pet.SizeId, ownerService.GetOwnerId(this.User.GetId()));

            

            return this.RedirectToAction("All");
        }


    }
}
