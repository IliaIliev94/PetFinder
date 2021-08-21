using PetFinder.Models.Shared;
using PetFinder.Services.Resources.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Resources
{
    public class AllResourcePostsViewModel
    {

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public IEnumerable<ResourcePostServiceModel> ResourcePosts { get; set; }

        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel { PostsPerPage = 8 };
    }
}
