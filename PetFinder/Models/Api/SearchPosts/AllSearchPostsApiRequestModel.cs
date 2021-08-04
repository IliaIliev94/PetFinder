using PetFinder.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Api.SearchPosts
{
    public class AllSearchPostsApiRequestModel
    {
        public string Type { get; init; }

        public string SearchTerm { get; init; }

        public string Species { get; init; }

        public string Size { get; init; }

        public SearchPostSorting Sorting { get; init; }
    }
}
