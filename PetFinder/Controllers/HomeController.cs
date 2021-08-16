using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetFinder.Data;
using PetFinder.Models;
using PetFinder.Services.SearchPosts;
using PetFinder.Services.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly ISearchPostService searchPostService;

        public HomeController(IStatisticsService statistics, ISearchPostService searchPostService)
        {
            this.statistics = statistics;
            this.searchPostService = searchPostService;
        }

        public IActionResult Index()
        {

            var searchPosts = this.searchPostService.Latest();

            var totalStatistics = this.statistics.Total();


            return View(searchPosts);
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
