using PetFinder.Models.Shared;
using PetFinder.Services.SearchPosts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.SearchPosts
{
    public class SearchPostDetailsViewModel
    {
        public SearchPostDetailsServiceModel SearchPost { get; set; }

        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel { PostsPerPage = 5 };
    }
}
