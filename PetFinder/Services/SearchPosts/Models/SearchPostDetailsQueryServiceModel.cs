using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.SearchPosts.Models
{
    public class SearchPostDetailsQueryServiceModel
    {
        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
        public SearchPostDetailsServiceModel SearchPost { get; set; }

    }
}
