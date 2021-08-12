using PetFinder.Models.Shared;
using PetFinder.Services.SearchPosts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.SearchPosts
{
    public class AllSearchPostsViewModel
    {
        public const int SearchPostsPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

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

        [Display(Name = "City")]
        public string City { get; init; }

        public IEnumerable<string> Cities { get; set; }

        public IEnumerable<SearchPostServiceModel> SearchPosts { get; set; }
    }
}
