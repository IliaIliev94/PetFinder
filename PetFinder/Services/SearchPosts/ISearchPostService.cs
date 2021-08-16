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
            string city,
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
            int? ownerId,
            string userId);

        bool Edit(
            string id,
            string title,
            string description,
            int cityId,
            DateTime? dateLostFound,
            string petId);

        bool Edit(
            string id,
            string title,
            string description,
            int cityId,
            DateTime? dateLostFound,
            string petName,
            string imageUrl,
            int petSpeciesId,
            int petSizeId);

        Tuple<bool, string> Delete(string id, string userId, bool userIsAdmin);

        IEnumerable<SearchPostServiceModel> GetSearchPostsById(string id);

        SearchPostDetailsServiceModel Details(string id);

        SearchPostEditServiceModel GetEditData(string id);

        IEnumerable<CityCategoryServiceModel> GetCities();

        IEnumerable<PetSelectServiceModel> GetPets(int ownerId);

        bool CityExists(int id);

        bool PetExists(string id);

        string GetUserId(string id);

        List<LatestSearchPostsServiceModel> Latest();
    }
}
