using PetFinder.Models.Shared;
using PetFinder.Services.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Resources
{
    public class ResourcesDetailsViewModel
    {
        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel { PostsPerPage = 5 };

        public ResourcePostDetailsServiceModel ResourcePost { get; set; }
    }
}
