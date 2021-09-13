using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Resources.Models
{
    public class ResourcePostDetailsQueryServiceModel
    {
        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public ResourcePostDetailsServiceModel ResourcePost { get; set; }
    }
}
