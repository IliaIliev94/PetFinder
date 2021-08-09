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

        IEnumerable<PetListServiceModel> All();

        PetDetailsServiceModel Details(string id);

        bool Edit(string id,
            string name,
            string imageUrl,
            int speciesId,
            int sizeId);

        EditPetServiceModel GetEditData(string id);

        ICollection<SizeCategoryServiceModel> GetSizes();

        ICollection<SpeciesCategoryServiceModel> GetSpecies();

    }
}
