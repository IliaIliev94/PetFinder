using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Api.Statistics
{
    public class StatisticsResponseModel
    {
        public int TotalPosts { get; init; }

        public int LostPets { get; init; }

        public int FoundPets { get; init; }
    }
}
