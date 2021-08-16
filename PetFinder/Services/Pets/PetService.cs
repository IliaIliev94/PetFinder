using PetFinder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetFinder.Data.Models;
using PetFinder.Services.Pets.Models;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace PetFinder.Services.Pets
{
    public class PetService : IPetService
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PetService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
                OwnerId = ownerId,
            };

            this.context.Pets.Add(pet);

            this.context.SaveChanges();

            return pet.Id;
        }

        public IEnumerable<PetServiceModel> All()
        {
            return this.context
                .Pets
                .ProjectTo<PetServiceModel>(mapper.ConfigurationProvider)
                .ToList();
        }

        public IEnumerable<PetServiceModel> All(int ownerId)
        {
            return this.context
                .Pets
                .Where(pet => pet.OwnerId == ownerId)
                .ProjectTo<PetServiceModel>(mapper.ConfigurationProvider)
                .ToList();
        }

        public PetDetailsServiceModel Details(string id)
        {
           return this.context.Pets
                .Where(pets => pets.Id == id)
                .ProjectTo<PetDetailsServiceModel>(mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public ICollection<SizeCategoryServiceModel> GetSizes()
        {
            return this.context.Sizes
                .ProjectTo<SizeCategoryServiceModel>(mapper.ConfigurationProvider)
                .ToList();
        }

        public ICollection<SpeciesCategoryServiceModel> GetSpecies()
        {
            var speciesList = this.context.Species
                .OrderByDescending(species => species.Id)
                .ProjectTo<SpeciesCategoryServiceModel>(mapper.ConfigurationProvider)
                .ToList();

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
                .ProjectTo<PetEditServiceModel>(mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public bool Delete(string id)
        {
            var pet = this.context.Pets.FirstOrDefault(pet => pet.Id == id);

            if(pet == null)
            {
                return false;
            }

            this.context.Pets.Remove(pet);

            this.context.SaveChanges();

            return true;
        }

        public bool Delete(string id, int ownerId)
        {
            var pet = this.context.Pets.FirstOrDefault(pet => pet.Id == id);

            if(pet.OwnerId != ownerId || context.SearchPosts.Any(searchPost => searchPost.PetId == pet.Id))
            {
                return false;
            }

            this.Delete(id);

            return true;
        }

        public bool SizeExists(int id)
        {
            return this.context.Sizes
                .Any(size => size.Id == id);
        }

        public bool SpeciesExists(int id)
        {
            return this.context.Species
                .Any(specie => specie.Id == id);
        }

        public int? GetOwnerId(string id)
        {
            return this.context
                .Pets
                .FirstOrDefault(pet => pet.Id == id)
                .OwnerId;
        }

        public int PetsCount()
        {
            return this.context.Pets.Count();
        }
    }
}
