using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Resources.Models
{
    public class ResourcePostServiceModel
    {
        public string Id { get; init; }

        public string Title { get; init; }

        public string ImageUrl { get; init; }

        public string Description { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}
