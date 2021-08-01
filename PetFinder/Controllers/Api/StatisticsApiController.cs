using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Models.Api.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public StatisticsApiController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var statisticsData = new StatisticsResponseModel
            {
                TotalPosts = context.SearchPosts.Count(),
                FoundPets = context.SearchPosts.Where(searchPost => searchPost.IsFound).Count(),
                LostPets = context.SearchPosts.Where(searchPost => !searchPost.IsFound).Count(),
            };

            return statisticsData;
        }

    }
}
