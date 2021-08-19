using FluentAssertions;
using PetFinder.Services.Statistics;
using PetFinder.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using static PetFinder.Tests.Data.SearchPostsData;

namespace PetFinder.Tests.Services
{
    public class StatisticsServiceTest
    {
        private IStatisticsService statisticsService;

        [Fact]
        public void TotalShouldReturnAccurateResult()
        {
            var database = DatabaseMock.Instance;
            this.statisticsService = new StatisticsService(database);

            database.SearchPosts.AddRange(GetLostSearchPosts());
            database.SaveChanges();

            var total = statisticsService.Total();
            Assert.Equal(10, total.TotalPosts);
            Assert.Equal(10, total.LostPets);
        }
    }
}
