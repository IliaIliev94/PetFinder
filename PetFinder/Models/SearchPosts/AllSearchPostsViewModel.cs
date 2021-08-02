using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.SearchPosts
{
    public class AllSearchPostsViewModel
    {
        public string Type { get; init; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public string Species { get; init; }

        public IEnumerable<string> PetSpecies { get; init; }

        public string Size { get; init; }

        public IEnumerable<string> PetSizes { get; init; }

        public SearchPostSorting Sorting { get; init; }

        public IEnumerable<SearchPostListViewModel> SearchPosts { get; init; }
    }
}
