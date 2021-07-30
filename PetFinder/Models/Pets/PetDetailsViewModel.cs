using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Pets
{
    public class PetDetailsViewModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public string Species { get; init; }

        public string Size { get; init; }
    }
}
