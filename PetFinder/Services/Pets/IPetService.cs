using PetFinder.Services.Pets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Pets
{
    public interface IPetService
    {

        string Create (string name,
            string imageUrl,
            int speciesId,
            int sizeId,
            int? ownerId);

        IEnumerable<PetServiceModel> All(int ownerId);

        PetDetailsServiceModel Details(string id);

        bool Edit(string id,
            string name,
            string imageUrl,
            int speciesId,
            int sizeId);

        bool Delete(string id, int ownerId);

        PetEditServiceModel GetEditData(string id);

        ICollection<SizeCategoryServiceModel> GetSizes();

        ICollection<SpeciesCategoryServiceModel> GetSpecies();

        bool SizeExists(int id);

        bool SpeciesExists(int id);

    }
}
