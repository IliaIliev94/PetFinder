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
using AutoMapper;

namespace PetFinder.Controllers
{
    public class SearchPostsController : Controller
    {

        private readonly ISearchPostService searchPostService;
        private readonly IPetService petService;
        private readonly IOwnerService ownerService;
        private readonly IMapper mapper;
        public SearchPostsController(ISearchPostService searchPostService, IPetService petService, IOwnerService ownerService, IMapper mapper)
        {
            this.searchPostService = searchPostService;
            this.petService = petService;
            this.ownerService = ownerService;
            this.mapper = mapper;
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
                query.City,
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
        public IActionResult Mine()
        {
            var searchPosts = this.searchPostService.GetSearchPostsById(this.User.GetId());

            return this.View(searchPosts);
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
                Pets = this.ownerService.IsOwner(this.User.GetId()) ? this.searchPostService.GetPets(this.ownerService.GetOwnerId(this.User.GetId())) : null,
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

            if(!searchPostService.CityExists(searchPost.CityId))
            {
                this.ModelState.AddModelError(nameof(searchPost.CityId), "City does not exist.");
            }

            if(searchPost.PetId != "0" && !this.searchPostService.PetExists(searchPost.PetId))
            {
                this.ModelState.AddModelError(nameof(searchPost.PetId), "Pet does not exist.");
            }

            if(!ModelState.IsValid)
            {
                searchPost.Cities = this.searchPostService.GetCities();
                searchPost.Pets = this.searchPostService.GetPets(this.ownerService.GetOwnerId(this.User.GetId()));
                searchPost.Pet.Species = this.petService.GetSpecies();
                searchPost.Pet.Sizes = this.petService.GetSizes();

                ViewBag.Type = searchPost.SearchPostType;

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

            if(searchPost == null || searchPost.PetId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            if(this.searchPostService.GetUserId(id) != this.User.GetId() && !User.IsAdmin())
            {
                return this.BadRequest();
            }

            var searchPostFormModel = this.mapper.Map<AddSearchPostFormModel>(searchPost);

            searchPostFormModel.Cities = this.searchPostService.GetCities();
            if(!this.User.IsAdmin())
            {
                searchPostFormModel.Pets = this.searchPostService.GetPets(this.ownerService.GetOwnerId(this.User.GetId()));
            }
            searchPostFormModel.Pet = searchPost.Type == "Lost" ? null : GetEditPetData(searchPost.PetId);

            return this.View(searchPostFormModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(AddSearchPostFormModel searchPost)
        {

            if(this.searchPostService.GetUserId(searchPost.Id) != this.User.GetId() && !this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            if (searchPost.SearchPostType == "Lost" && searchPost.PetId != "0")
            {
                ModelState.Remove("Pet.ImageUrl");
            }

            if (!searchPostService.CityExists(searchPost.CityId) && !User.IsAdmin())
            {
                this.ModelState.AddModelError(nameof(searchPost.CityId), "City does not exist.");
            }

            if (!ModelState.IsValid)
            {
                searchPost.Cities = this.searchPostService.GetCities();
                if(!this.User.IsAdmin())
                {
                    searchPost.Pets = this.searchPostService.GetPets(this.ownerService.GetOwnerId(this.User.GetId()));
                }
                
                return this.View(searchPost);
            }

            var isEditSuccessfull = false;

            if(this.User.IsAdmin())
            {
                isEditSuccessfull = this.searchPostService.Edit(searchPost.Id, searchPost.Title, searchPost.Description, 0, searchPost.DateLostFound, null);
            }
            else
            {
                if (searchPost.Pet == null)
                {
                    isEditSuccessfull = this.searchPostService.Edit(searchPost.Id, searchPost.Title, searchPost.Description, searchPost.CityId, searchPost.DateLostFound, searchPost.PetId);
                }
                else
                {
                    isEditSuccessfull = this.searchPostService.Edit(searchPost.Id, searchPost.Title, searchPost.Description,
                        searchPost.CityId, searchPost.DateLostFound, searchPost.Pet.Name,
                        searchPost.Pet.ImageUrl, searchPost.Pet.SpeciesId, searchPost.Pet.SizeId);
                }
            }

            if(!isEditSuccessfull)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction("Details", "SearchPosts", new { Id = searchPost.Id });
        }

        [Authorize]
        public IActionResult Delete(string id)
        {

            var isDeleteSuccessfull = this.searchPostService.Delete(id, this.User.GetId(), this.User.IsAdmin());

            if(!isDeleteSuccessfull.Item1)
            {
                return this.BadRequest();
            }

            if(this.User.IsAdmin())
            {
                return this.RedirectToAction("All", "SearchPosts", new { Type = isDeleteSuccessfull.Item2 });
            }

            return this.RedirectToAction("Mine");
        }


        private void SetAllSearchPostQueryRsponseData(AllSearchPostsViewModel query, SearchPostQueryServiceModel queryResult)
        {
            query.PetSizes = queryResult.PetSizes;
            query.PetSpecies = queryResult.PetSpecies;
            query.SearchPosts = queryResult.SearchPosts;
            query.TotalPages = queryResult.TotalPages;
            query.CurrentPage = queryResult.CurrentPage;
            query.Cities = queryResult.Cities;
        }

        private AddPetFormModel GetEditPetData(string id)
        {
            var pet = this.mapper.Map<AddPetFormModel>(this.petService.GetEditData(id));
            pet.Sizes = this.petService.GetSizes();
            pet.Species = this.petService.GetSpecies();

            return pet;
        }

    }
}
