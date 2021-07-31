using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetFinder.Data;
using PetFinder.Models;
using PetFinder.Models.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            this._logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {

            var indexModel = new IndexViewModel
            {
                TotalPosts = this.context.SearchPosts.Count(),

                FoundPets = this.context.SearchPosts
                    .Where(searchPost => searchPost.IsFound)
                    .Count(),

                LostPets = this.context.SearchPosts
                    .Where(searchPost => !searchPost.IsFound)
                    .Count(),

                Pets = this.context.Pets
                    .OrderByDescending(pet => pet.Id)
                    .Select(pet => new PetHomeViewModel
                    {
                        Id = pet.Id,
                        Name = pet.Name,
                        ImageUrl = pet.ImageUrl,
                        Species = pet.Species.Name,
                    })
                    .Take(3)
                    .ToList(),
            };

            return View(indexModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
