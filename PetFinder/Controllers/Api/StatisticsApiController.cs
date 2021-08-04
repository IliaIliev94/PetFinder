using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Models.Api.Statistics;
using PetFinder.Services.Statistics;
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
        private readonly IStatisticsService statistics;

        public StatisticsApiController(ApplicationDbContext context, IStatisticsService statistics)
        {
            this.context = context;
            this.statistics = statistics;
        }

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var totalStatistics = statistics.Total();

            var statisticsData = new StatisticsResponseModel
            {
                TotalPosts = totalStatistics.TotalPosts,
                FoundPets = totalStatistics.FoundPets,
                LostPets = totalStatistics.LostPets,
            };

            return statisticsData;
        }

    }
}
