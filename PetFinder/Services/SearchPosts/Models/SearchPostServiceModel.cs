using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.SearchPosts.Models
{
    public class SearchPostServiceModel
    {
        public string Id { get; init; }

        public string Title { get; init; }

        public string ImageUrl { get; init; }

        public string PetName { get; init; }

        public string PetSpecies { get; init; }

        public bool IsSaved { get; init; }

        public string UserId { get; init; }
    }
}
