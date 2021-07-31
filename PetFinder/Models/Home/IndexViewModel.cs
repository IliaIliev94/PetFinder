using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Home
{
    public class IndexViewModel
    {
        public int TotalPosts { get; init; }

        public int LostPets { get; init; }

        public int FoundPets { get; init; }

        public IList<PetHomeViewModel> Pets { get; init; }
    }
}
