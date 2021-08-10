using PetFinder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetFinder.Data.Models;
using PetFinder.Services.Pets.Models;

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
                int? ownerId)
        {
            var pet = new Pet
            {
                Name = name,
                ImageUrl = imageUrl,
                SpeciesId = speciesId,
                SizeId = sizeId,
                OwnerId = null,
            };

            this.context.Pets.Add(pet);

            this.context.SaveChanges();

            return pet.Id;
        }

        public IEnumerable<PetListServiceModel> All()
        {
            return this.context
                .Pets
                .Select(pet => new PetListServiceModel
                {
                    Id = pet.Id,
                    Name = pet.Name,
                    ImageUrl = pet.ImageUrl,
                    Species = pet.Species.Name,
                    Size = pet.Size.Type,
                })
                .ToList();
        }

        public PetDetailsServiceModel Details(string id)
        {
           return this.context.Pets
                .Where(pets => pets.Id == id)
                .Select(pets => new PetDetailsServiceModel
                {
                    Id = pets.Id,
                    Name = pets.Name,
                    ImageUrl = pets.ImageUrl,
                    Species = pets.Species.Name,
                    Size = pets.Size.Type,
                })
                .FirstOrDefault();
        }

        public ICollection<SizeCategoryServiceModel> GetSizes()
        {
            return this.context.Sizes.Select(size => new SizeCategoryServiceModel { Id = size.Id, Type = size.Type }).ToList();
        }

        public ICollection<SpeciesCategoryServiceModel> GetSpecies()
        {
            var speciesList = this.context.Species.Select(specie => new SpeciesCategoryServiceModel { Id = specie.Id, Name = specie.Name }).ToList();
            speciesList.Reverse();

            return speciesList;
        }

        public bool Edit(
            string id, 
            string name,
            string imageUrl,
            int speciesId,
            int sizeId)
        {
            var pet = this.context.Pets.FirstOrDefault(pet => pet.Id == id);

            if(pet == null)
            {
                return false;
            }

            pet.Name = name;
            pet.ImageUrl = imageUrl;
            pet.SpeciesId = speciesId;
            pet.SizeId = sizeId;

            this.context.SaveChanges();

            return true;
        }

        public PetEditServiceModel GetEditData(string id)
        {
            return this.context.Pets
                .Where(pet => pet.Id == id)
                .Select(pet => new PetEditServiceModel
                {
                    Name = pet.Name,
                    ImageUrl = pet.ImageUrl,
                    SizeId = pet.SizeId,
                    SpeciesId = pet.SpeciesId,
                    OwnerId = pet.OwnerId,
                })
                .FirstOrDefault();
        }
    }
}
