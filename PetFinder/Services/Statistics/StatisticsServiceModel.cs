using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Statistics
{
    public class StatisticsServiceModel
    {
        public int TotalPosts { get; init; }

        public int LostPets { get; init; }

        public int FoundPets { get; init; }
    }
}
