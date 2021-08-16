using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Models.Pets;
using PetFinder.Services.Pets;
using PetFinder.Services.Owners;
using PetFinder.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace PetFinder.Controllers
{
    public class PetsController : Controller
    {
        private readonly IPetService petService;
        private readonly IOwnerService ownerService;
        private readonly IMapper mapper;

        public PetsController(IPetService petService, IOwnerService ownerService, IMapper mapper)
        {
            this.petService = petService;
            this.ownerService = ownerService;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult All()
        {
            var userId = this.User.GetId();

            var petsCount = this.petService.PetsCount();

            if (!this.User.IsAdmin() && !this.ownerService.IsOwner(userId))
            {
                return this.Unauthorized();
            }

            var pets = this.User.IsAdmin() ? this.petService.All() : this.petService.All(this.ownerService.GetOwnerId(this.User.GetId()));

            return this.View(pets);
        }

        [Authorize]
        public IActionResult Details(string id)
        {

            if (!this.User.IsAdmin() && !this.ownerService.IsOwner(this.User.GetId()))
            {
                return this.Unauthorized();
            }

            var pet = this.petService.Details(id);

            if(pet == null)
            {
                return this.NotFound();
            }

            return this.View(pet);

        }

        [Authorize]
        public IActionResult Add(string searchId, int ownerId)
        {

            if (!this.ownerService.IsOwner(this.User.GetId()))
            {
                return RedirectToAction("Become", "Owners");
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
        [Authorize]
        public IActionResult Add(AddPetFormModel pet)
        {

            if (!this.ownerService.IsOwner(this.User.GetId()))
            {
                return this.BadRequest();
            }

            if (!this.petService.SizeExists(pet.SizeId))
            {
                ModelState.AddModelError(nameof(pet.SizeId), "Size does not exist.");
            }

            if (!this.petService.SpeciesExists(pet.SpeciesId))
            {
                ModelState.AddModelError(nameof(pet.SpeciesId), "Species does not exist.");
            }

            if (!ModelState.IsValid)
            {
                pet.Sizes = this.petService.GetSizes();
                pet.Species = this.petService.GetSpecies();

                return this.View(pet);
            }


            var newPet = this.petService.Create(pet.Name, pet.ImageUrl, pet.SpeciesId, pet.SizeId, ownerService.GetOwnerId(this.User.GetId()));

            

            return this.RedirectToAction("Details", new { Id = newPet });
        }

        [Authorize]
        public IActionResult Edit(string id)
        {

            var pet = this.petService.GetEditData(id);
            var userId = this.User.GetId();

            if(pet == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            if (!this.User.IsAdmin() && (!this.ownerService.IsOwner(userId) || this.ownerService.GetOwnerId(userId) != this.petService.GetOwnerId(id)))
            {
                return this.BadRequest();
            }

            var petFormModel = this.mapper.Map<AddPetFormModel>(pet);
            petFormModel.Sizes = this.petService.GetSizes();
            petFormModel.Species = this.petService.GetSpecies();

            return this.View(petFormModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(AddPetFormModel pet)
        {
            var userId = this.User.GetId();

            if (!this.User.IsAdmin() && (!this.ownerService.IsOwner(userId) || this.ownerService.GetOwnerId(userId) != this.petService.GetOwnerId(pet.Id)))
            {
                return this.BadRequest();
            }

            if(!this.petService.SizeExists(pet.SizeId))
            {
                ModelState.AddModelError(nameof(pet.SizeId), "Size does not exist.");
            }

            if(!this.petService.SpeciesExists(pet.SpeciesId))
            {
                ModelState.AddModelError(nameof(pet.SpeciesId), "Species does not exist.");
            }

            if (!ModelState.IsValid)
            {
                pet.Sizes = this.petService.GetSizes();
                pet.Species = this.petService.GetSpecies();

                return this.View(pet);
            }

            var successFull = this.petService.Edit(
                pet.Id, 
                pet.Name,
                pet.ImageUrl,
                pet.SpeciesId,
                pet.SizeId);

            if(!successFull)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Pets", new { Id = pet.Id });
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var isDeleteSuccessfull = this.User.IsAdmin() ? this.petService.Delete(id) : this.petService.Delete(id, this.ownerService.GetOwnerId(this.User.GetId()));

            if(!isDeleteSuccessfull)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction("All");
        }
    }
}
