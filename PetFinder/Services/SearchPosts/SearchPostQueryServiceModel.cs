using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.SearchPosts
{
    public class SearchPostQueryServiceModel
    {
        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
        public IEnumerable<string> PetSizes { get; init; }

        public IEnumerable<string> PetSpecies { get; init; }
        public IEnumerable<SearchPostServiceModel> SearchPosts { get; set; }
    }
}
