using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Data.Models;
using PetFinder.Models.Cities;
using PetFinder.Models.Pets;
using PetFinder.Models.SearchPosts;
using PetFinder.Models.Shared;
using PetFinder.Services.SearchPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PetFinder.Infrastructure;
using PetFinder.Models.Sizes;
using PetFinder.Models.Species;
using PetFinder.Services.Pets;

namespace PetFinder.Controllers
{
    public class SearchPostsController : Controller
    {

        private ApplicationDbContext context;
        private ISearchPostService searchPostService;
        private IPetService petService;

        public SearchPostsController(ApplicationDbContext context, ISearchPostService searchPostService, IPetService petService)
        {
            this.context = context;
            this.searchPostService = searchPostService;
            this.petService = petService;
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
            var searchPost = this.context
                .SearchPosts
                .Where(searchPost => searchPost.Id == id)
                .Select(searchPost => new SearchPostDetailsViewModel
                {
                    Id = searchPost.Id,
                    Title = searchPost.Title,
                    Description = searchPost.Description,
                    ImageUrl = searchPost.Pet.ImageUrl,
                    City = searchPost.City.Name,
                    PetName = searchPost.Pet.Name,
                    PetSpecies = searchPost.Pet.Species.Name,
                })
                .FirstOrDefault();

            if (searchPost == null)
            {
                return this.BadRequest();
            }


            return this.View(searchPost);
        }

        [Authorize]
        public IActionResult Add(string type)
        {
            if(type == "Lost")
            {

                if(!this.UserIsOwner())
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
                Pets = GetPets(),
                Cities = GetCities(),
                Pet = new AddPetFormModel
                {
                    Species = this.GetSpecies(),
                    Sizes = this.GetSizes(),
                }
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddSearchPostFormModel searchPost)
        {

            var owner = this.context.Owners.FirstOrDefault(owner => owner.UserId == this.User.GetId());

            if(searchPost.SearchPostType == "Lost" && owner == null)
            {
                return this.RedirectToAction("Become", "Owners");
            }

            if(searchPost.SearchPostType == "Lost" && searchPost.PetId != "0")
            {
                ModelState.Remove("Pet.ImageUrl");
            }

            if(!ModelState.IsValid)
            {
                searchPost.Cities = GetCities();
                searchPost.Pets = GetPets();
                searchPost.Pet.Species = GetSpecies();
                searchPost.Pet.Sizes = GetSizes();

                return this.View(searchPost);
            }

            var newSearchPost = new SearchPost
            {
                Title = searchPost.Title,
                Description = searchPost.Description,
                IsFound = searchPost.SearchPostType == "Found" ? true : false,
                CityId = searchPost.CityId,
                DatePublished = DateTime.UtcNow,
                DateLostFound = searchPost.DateLostFound,
                SearchPostTypeId = searchPost.SearchPostType == "Found" ? 1 : 2,
                PetId = (searchPost.SearchPostType == "Found" || searchPost.PetId == "0") ? CreatePet(searchPost, owner) : searchPost.PetId,
            };



            this.context.SearchPosts.Add(newSearchPost);

            this.context.SaveChanges();

            return this.RedirectToAction("All", "SearchPosts", new { Type = searchPost.SearchPostType});
        }

        private IEnumerable<CityViewModel> GetCities()
        {
            return this.context.Cities.Select(city => new CityViewModel { Id = city.Id, Name = city.Name }).ToList();
        }

        private IEnumerable<PetListViewModel> GetPets()
        {
            return this.context.Pets.Select(pet => new PetListViewModel { Id = pet.Id, Name = pet.Name }).ToList();
        }

        private ICollection<SizeViewModel> GetSizes()
        {
            return this.context.Sizes.Select(size => new SizeViewModel { Id = size.Id, Type = size.Type }).ToList();
        }

        private ICollection<SpeciesViewModel> GetSpecies()
        {
            var speciesList = this.context.Species.Select(specie => new SpeciesViewModel { Id = specie.Id, Name = specie.Name }).ToList();
            speciesList.Reverse();

            return speciesList;
        }

        private void SetAllSearchPostQueryRsponseData(AllSearchPostsViewModel query, SearchPostQueryServiceModel queryResult)
        {
            query.PetSizes = queryResult.PetSizes;
            query.PetSpecies = queryResult.PetSpecies;
            query.SearchPosts = queryResult.SearchPosts;
            query.TotalPages = queryResult.TotalPages;
            query.CurrentPage = queryResult.CurrentPage;
        }

        private string CreatePet(AddSearchPostFormModel searchPost, Owner owner)
        {
            if(searchPost.SearchPostType == "Found")
            {
                return petService.Create(searchPost.Pet.Name, searchPost.Pet.ImageUrl, searchPost.Pet.SpeciesId, searchPost.Pet.SizeId);
            }
            return petService.Create(searchPost.Pet.Name, searchPost.Pet.ImageUrl, searchPost.Pet.SpeciesId, searchPost.Pet.SizeId, owner.Id);
        }

        private bool UserIsOwner()
        {
            return this.context.Owners.Any(owner => owner.UserId == this.User.GetId());
        }
    }
}
