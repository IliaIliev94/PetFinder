using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Models.Pets;
using PetFinder.Models.SearchPosts;
using PetFinder.Services.SearchPosts;
using System.Linq;
using PetFinder.Infrastructure;
using PetFinder.Services.Pets;
using PetFinder.Services.SearchPosts.Models;
using PetFinder.Services.Owners;

namespace PetFinder.Controllers
{
    public class SearchPostsController : Controller
    {

        private readonly ISearchPostService searchPostService;
        private readonly IPetService petService;
        private readonly IOwnerService ownerService;

        public SearchPostsController(ISearchPostService searchPostService, IPetService petService, IOwnerService ownerService)
        {
            this.searchPostService = searchPostService;
            this.petService = petService;
            this.ownerService = ownerService;
        }

        public IActionResult All([FromQuery] AllSearchPostsViewModel query)
        {

            if (query.Type != "Lost" && query.Type != "Found")
            {
                return this.BadRequest();
            }

            var queryResult = this.searchPostService.All(
                query.Species,
                query.Size,
                query.SearchTerm,
                query.Type,
                query.CurrentPage,
                AllSearchPostsViewModel.SearchPostsPerPage,
                query.Sorting);

            SetAllSearchPostQueryRsponseData(query, queryResult);

            return this.View(query);
        }

        public IActionResult Details(string id)
        {
            var searchPost = this.searchPostService.Details(id);

            if (searchPost == null)
            {
                return this.RedirectToAction("Error", "Home");
            }


            return this.View(searchPost);
        }

        [Authorize]
        public IActionResult Add(string type)
        {
            if(type == "Lost")
            {

                if(!this.ownerService.IsOwner(this.User.GetId()))
                {
                    return this.RedirectToAction("Become", "Owners");
                }

                ViewBag.Type = "Lost";
            }
            else
            {
                ViewBag.Type = "Found";
            }

            return this.View(new AddSearchPostFormModel 
            { 
                Pets = this.searchPostService.GetPets(),
                Cities = this.searchPostService.GetCities(),
                Pet = new AddPetFormModel
                {
                    Species = this.petService.GetSpecies(),
                    Sizes = this.petService.GetSizes(),
                }
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddSearchPostFormModel searchPost)
        {

            if(searchPost.SearchPostType == "Lost" && !this.ownerService.IsOwner(this.User.GetId()))
            {
                return this.RedirectToAction("Become", "Owners");
            }

            if(searchPost.SearchPostType == "Lost" && searchPost.PetId != "0")
            {
                ModelState.Remove("Pet.ImageUrl");
            }

            if(!ModelState.IsValid)
            {
                searchPost.Cities = this.searchPostService.GetCities();
                searchPost.Pets = this.searchPostService.GetPets();
                searchPost.Pet.Species = this.petService.GetSpecies();
                searchPost.Pet.Sizes = this.petService.GetSizes();

                return this.View(searchPost);
            }

            var userId = this.User.GetId();
            int? ownerId = this.ownerService.IsOwner(userId) ? this.ownerService.GetOwnerId(userId) : null;

            searchPostService.Create(
                searchPost.Title,
                searchPost.Description,
                searchPost.SearchPostType,
                searchPost.CityId,
                searchPost.DateLostFound,
                searchPost.PetId,
                searchPost.Pet.Name,
                searchPost.Pet.ImageUrl,
                searchPost.Pet.SpeciesId,
                searchPost.Pet.SizeId,
                ownerId,
                userId);

            return this.RedirectToAction("All", "SearchPosts", new { Type = searchPost.SearchPostType});
        }


        [Authorize]
        public IActionResult Edit(string id)
        {

            var searchPost = this.searchPostService.GetEditData(id);

            if(searchPost == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            if(searchPost.UserId != this.User.GetId())
            {
                return this.BadRequest();
            }

            return this.View(new AddSearchPostFormModel 
            {
                Id = id,
                Title = searchPost.Title,
                Description = searchPost.Description,
                DateLostFound = searchPost.DateLostFound,
                CityId = searchPost.CityId,
                Cities = this.searchPostService.GetCities(),
                PetId = searchPost.PetId,
                Pets = this.searchPostService.GetPets(),
                Pet = searchPost.Type == "Lost" ? null : GetEditPetData(searchPost.PetId),
                UserId = searchPost.UserId,
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(AddSearchPostFormModel searchPost)
        {
            if(!ModelState.IsValid)
            {
                searchPost.Cities = this.searchPostService.GetCities();
                searchPost.Pets = this.searchPostService.GetPets();
                return this.View(searchPost);
            }

            var isEditSuccessfull = false;

            if(searchPost.Pet == null)
            {
                isEditSuccessfull = this.searchPostService.Edit(searchPost.Id, searchPost.Title, searchPost.Description, searchPost.CityId, searchPost.DateLostFound, searchPost.PetId);
            }
            else
            {
                isEditSuccessfull = this.searchPostService.Edit(searchPost.Id, searchPost.Title, searchPost.Description, 
                    searchPost.CityId, searchPost.DateLostFound, searchPost.PetId, searchPost.Pet.Name, 
                    searchPost.Pet.ImageUrl, searchPost.Pet.SpeciesId, searchPost.Pet.SizeId);
            }

            if(!isEditSuccessfull)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction("Details", "SearchPosts", new { Id = searchPost.Id });
        }

        private void SetAllSearchPostQueryRsponseData(AllSearchPostsViewModel query, SearchPostQueryServiceModel queryResult)
        {
            query.PetSizes = queryResult.PetSizes;
            query.PetSpecies = queryResult.PetSpecies;
            query.SearchPosts = queryResult.SearchPosts;
            query.TotalPages = queryResult.TotalPages;
            query.CurrentPage = queryResult.CurrentPage;
        }

        private AddPetFormModel GetEditPetData(string id)
        {
            var pet = this.petService.GetEditData(id);

            return new AddPetFormModel
            {
                Id = id,
                Name = pet.Name,
                ImageUrl = pet.ImageUrl,
                SizeId = pet.SizeId,
                Sizes = this.petService.GetSizes(),
                SpeciesId = pet.SpeciesId,
                Species = this.petService.GetSpecies(),
            };
        }

    }
}
