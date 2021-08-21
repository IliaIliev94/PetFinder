using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Shared
{
    public class PaginationViewModel
    {
        public string Type { get; set; }

        public int PostsPerPage { get; set; } = 9;

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }
    }
}
