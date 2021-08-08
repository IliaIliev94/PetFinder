using PetFinder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetFinder.Data.Models;

namespace PetFinder.Services.Pets
{
    public class PetService : IPetService
    {

        private readonly ApplicationDbContext context;

        public PetService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string Create(
                string name,
                string imageUrl,
                int speciesId,
                int sizeId,
                int ownerId)
        {
            var pet = new Pet
            {
                Name = name,
                ImageUrl = imageUrl,
                SpeciesId = speciesId,
                SizeId = sizeId,
                OwnerId = ownerId == 0 ? null: ownerId,
            };

            this.context.Pets.Add(pet);

            this.context.SaveChanges();

            return pet.Id;
        }

        public string Create(string name, string imageUrl, int speciesId, int sizeId)
        {
            return this.Create(name, imageUrl, speciesId, sizeId, 0);
        }
    }
}
