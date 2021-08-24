using PetFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFinder.Tests.Data
{
    public static class PetData
    {
        public static Pet GetPet(string petId, string name, int sizeId, int speciesId, string imageUrl, int? ownerId)
        {
            return new Pet { Id = petId, Name = name, SizeId = sizeId, SpeciesId = speciesId, ImageUrl = imageUrl, OwnerId = ownerId };
        }

        public static IEnumerable<Pet> GetPets()
        {
            return Enumerable.Range(0, 10).Select(i => new Pet { Id = i.ToString(), Name = i.ToString() });
        }
    }
}
