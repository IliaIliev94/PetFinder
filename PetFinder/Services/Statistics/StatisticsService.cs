using PetFinder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext context;

        public StatisticsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public StatisticsServiceModel Total()
        {
            var totalPosts = context.SearchPosts.Count();
            var lostPets = context.SearchPosts.Where(searchPost => !searchPost.IsFound).Count();
            var foundPets = context.SearchPosts.Where(searchPost => searchPost.IsFound).Count();

            return new StatisticsServiceModel
            {
                TotalPosts = totalPosts,
                LostPets = lostPets,
                FoundPets = foundPets,
            };
        }
    }
}
