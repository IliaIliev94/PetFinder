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

            var newSearchPost = new SearchPost
            {
                Title = searchPost.Title,
                Description = searchPost.Description,
                CityId = searchPost.CityId,
                DatePublished = DateTime.UtcNow,
                DateLostFound = searchPost.DateLostFound,
                SearchPostTypeId = searchPost.SearchPostType == "Lost" ? 1 : 2,
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
