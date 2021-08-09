using PetFinder.Models.Shared;
using PetFinder.Services.SearchPosts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.SearchPosts
{
    public interface ISearchPostService
    {
        SearchPostQueryServiceModel All(string species,
            string size,
            string searchTerm,
            string type,
            int currentPage,
            int searchPostsPerPage,
            SearchPostSorting sorting);

        string Create(
            string title,
            string description,
            string searchPostType,
            int cityId,
            DateTime? dateLostFound,
            string petId,
            string petName,
            string imageUrl,
            int speciesId,
            int sizeId,
            int? ownerId);

        SearchPostDetailsServiceModel Details(string id);

        IEnumerable<CityCategoryServiceModel> GetCities();

        IEnumerable<PetSelectServiceModel> GetPets();
    }
}
