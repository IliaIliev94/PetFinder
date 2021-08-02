using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.SearchPosts
{
    public class SearchPostListViewModel
    {
        public string Id { get; init; }

        public string Title { get; init; }

        public string ImageUrl { get; init; }

        public string PetName { get; init; }

        public string PetSpecies { get; init; }

    }
}
