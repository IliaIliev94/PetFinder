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

        [Display(Name = "Pet Species")]
        public string Species { get; init; }

        public IEnumerable<string> PetSpecies { get; set; }

        [Display(Name = "Pet Size")]
        public string Size { get; init; }

        public IEnumerable<string> PetSizes { get; set; }

        [Display(Name = "Sorting")]
        public SearchPostSorting Sorting { get; init; }

        public IEnumerable<SearchPostListViewModel> SearchPosts { get; set; }
    }
}
