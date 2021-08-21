using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Resources.Models
{
    public class ResourcePostQueryServiceModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
        public IEnumerable<ResourcePostServiceModel> Resources { get; set; }
    }
}
