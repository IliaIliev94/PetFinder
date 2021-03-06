using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Pets.Models
{
    public class PetEditServiceModel
    {
        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public int SizeId { get; init; }

        public int SpeciesId { get; init; }

        public int? OwnerId { get; init; }

    }
}
