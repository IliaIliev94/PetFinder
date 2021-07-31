using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Data.Models;
using PetFinder.Models.Cities;
using PetFinder.Models.Pets;
using PetFinder.Models.SearchPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PetFinder.Controllers
{
    public class SearchPostsController : Controller
    {

        private ApplicationDbContext context;

        public SearchPostsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult All(string type)
        {
            var searchPosts = this.context
                .SearchPosts
                .Where(searchPost => searchPost.SearchPostType.Name == type)
                .Select(searchPost => new SearchPostListViewModel
                {
                    Id = searchPost.Id,
                    Title = searchPost.Title,
                    ImageUrl = searchPost.Pet.ImageUrl,
                    PetName = searchPost.Pet.Name,
                    PetSpecies = searchPost.Pet.Species.Name,
                })
                .ToList();

            return this.View(searchPosts);
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

        public IActionResult Add(string type)
        {
            if(type == "Lost")
            {
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
            });
        }

        [HttpPost]
        public IActionResult Add(AddSearchPostFormModel searchPost)
        {

            if(!ModelState.IsValid)
            {
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
            };

            if (searchPost.SearchPostType != "Found" && searchPost.PetId != "0")
            {
                newSearchPost.PetId = searchPost.PetId;

                this.context.SearchPosts.Add(newSearchPost);

                this.context.SaveChanges();

                return this.RedirectToAction("Index", "Home");
            }



            this.context.SearchPosts.Add(newSearchPost);

            this.context.SaveChanges();

            

            return this.RedirectToAction("Add", "Pets", new { SearchId = newSearchPost.Id});
        }

        private IEnumerable<CityViewModel> GetCities()
        {
            return this.context.Cities.Select(city => new CityViewModel { Id = city.Id, Name = city.Name }).ToList();
        }

        private IEnumerable<PetListViewModel> GetPets()
        {
            return this.context.Pets.Select(pet => new PetListViewModel { Id = pet.Id, Name = pet.Name }).ToList();
        }
    }
}
