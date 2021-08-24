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

        public IActionResult All([FromQuery] AllSearchPostsViewModel query, int currentPage)
        {
            query.Pagination.CurrentPage = currentPage != 0 ? currentPage : 1;

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
                query.Pagination.CurrentPage,
                query.Pagination.PostsPerPage,
                query.Sorting,
                this.User.GetId());

            SetAllSearchPostQueryRsponseData(query, queryResult);

            return this.View(query);
        }

        public IActionResult Details(string id)
        {
            var searchPost = this.searchPostService.Details(id);

            if (searchPost == null)
            {
                return this.NotFound();
            }


            return this.View(searchPost);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var userId = this.User.GetId();

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

            if(searchPost.SearchPostType == "Lost")
            {
                if(!this.ownerService.IsOwner(this.User.GetId()))
                {
                    return this.RedirectToAction("Become", "Owners");
                }

                if(searchPost.PetId != "0")
                {
                    ModelState.Remove("Pet.ImageUrl");
                    ModelState.Remove("PhoneNumber");
                }

                if(searchPost.PetId != "0" && !this.searchPostService.PetExists(searchPost.PetId))
                {
                    this.ModelState.AddModelError(nameof(searchPost.PetId), "Pet does not exist.");
                }

                searchPost.PhoneNumber = this.ownerService.GetPhoneNumber(this.User.GetId());
                
            }


            if(!searchPostService.CityExists(searchPost.CityId))
            {
                this.ModelState.AddModelError(nameof(searchPost.CityId), "City does not exist.");
            }


            if(searchPost.PetId == "0")
            {
                if(!petService.SpeciesExists(searchPost.Pet.SpeciesId))
                {
                    this.ModelState.AddModelError(nameof(searchPost.Pet.SpeciesId), "Pet species does not exist.");
                }

                if (!petService.SizeExists(searchPost.Pet.SizeId))
                {
                    this.ModelState.AddModelError(nameof(searchPost.Pet.SizeId), "Pet size does not exist.");
                }
            }

            if(!ModelState.IsValid)
            {
                searchPost.Cities = this.searchPostService.GetCities();
                searchPost.Pets = searchPost.SearchPostType == "Lost"? this.searchPostService.GetPets(this.ownerService.GetOwnerId(this.User.GetId())) : null;
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
                userId,
                searchPost.PhoneNumber);

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
                return this.Unauthorized();
            }

            var searchPostFormModel = this.mapper.Map<AddSearchPostFormModel>(searchPost);

            searchPostFormModel.Cities = this.searchPostService.GetCities();

            searchPostFormModel.Pets = (!this.User.IsAdmin() && searchPost.Type == "Lost") 
                ? this.searchPostService.GetPets(this.ownerService.GetOwnerId(this.User.GetId())) : null;

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

            if(!this.searchPostService.SearchPostTypeExists(searchPost.SearchPostType))
            {
                return this.BadRequest();
            }

            if (searchPost.SearchPostType == "Lost")
            {
                ModelState.Remove("PhoneNumber");

                if(searchPost.PetId != "0")
                {
                    ModelState.Remove("Pet.ImageUrl");
                }
                
            }

            if (!searchPostService.CityExists(searchPost.CityId) && !User.IsAdmin())
            {
                this.ModelState.AddModelError(nameof(searchPost.CityId), "City does not exist.");
            }

            if (!ModelState.IsValid)
            {
                searchPost.Cities = this.searchPostService.GetCities();

                searchPost.Pets = this.User.IsAdmin()
                        ? null : this.searchPostService.GetPets(this.ownerService.GetOwnerId(this.User.GetId()));
                
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
                return this.NotFound();
            }

            return this.RedirectToAction("Details", "SearchPosts", new { Id = searchPost.Id });
        }

        [Authorize]
        public IActionResult Delete(string id)
        {

            var isDeleteSuccessfull = this.searchPostService.Delete(id, this.User.GetId(), this.User.IsAdmin());

            if(!isDeleteSuccessfull.Item1)
            {
                return this.NotFound();
            }

            if(this.User.IsAdmin())
            {
                return this.RedirectToAction("All", "SearchPosts", new { Type = isDeleteSuccessfull.Item2 });
            }

            return this.RedirectToAction("Mine");
        }

        [Authorize]
        public IActionResult SetAsFoundClaimed(string id)
        {

            if(!this.searchPostService.UserOwnsSearchPost(id, this.User.GetId()))
            {
                return this.Unauthorized();
            }

            var searchPostType = this.searchPostService.SetAsFoundClaimed(id);

            return this.RedirectToAction("All", new { Type = searchPostType });
        }


        private void SetAllSearchPostQueryRsponseData(AllSearchPostsViewModel query, SearchPostQueryServiceModel queryResult)
        {
            query.PetSizes = queryResult.PetSizes;
            query.PetSpecies = queryResult.PetSpecies;
            query.SearchPosts = queryResult.SearchPosts;
            query.Pagination.TotalPages = queryResult.TotalPages;
            query.Pagination.CurrentPage = queryResult.CurrentPage;
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
