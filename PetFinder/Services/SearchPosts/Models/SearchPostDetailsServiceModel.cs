using PetFinder.Services.Comments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.SearchPosts.Models
{
    public class SearchPostDetailsServiceModel
    {
        public string Id { get; init; }

        public string Title { get; init; }

        public string Description { get; init; }

        public string City { get; init; }

        public string ImageUrl { get; init; }

        public string PetName { get; init; }

        public string PetSpecies { get; init; }

        public string UserId { get; init; }

        public string PhoneNumber { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; init; }
    }
}
