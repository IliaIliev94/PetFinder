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
            SearchPostSorting sorting,
            string userId = null);

        IEnumerable<SearchPostServiceModel> Saved(string userId);

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
            string userId,
            string phoneNumber);

        bool Edit(
            string id,
            string title,
            string description,
            int cityId,
            DateTime? dateLostFound,
            string petId,
            string phoneNumber = null);

        bool Edit(
            string id,
            string title,
            string description,
            int cityId,
            DateTime? dateLostFound,
            string petName,
            string imageUrl,
            int petSpeciesId,
            int petSizeId,
            string phoneNumber);

        Tuple<bool, string> Delete(string id, string userId, bool userIsAdmin);

        string SetAsFoundClaimed(string id);

        IEnumerable<SearchPostServiceModel> GetSearchPostsById(string id);

        SearchPostDetailsQueryServiceModel Details(string id, int currentPage, int commentsPerPage);

        SearchPostEditServiceModel GetEditData(string id);

        IEnumerable<CityCategoryServiceModel> GetCities();

        IEnumerable<PetSelectServiceModel> GetPets(int? ownerId);

        bool CityExists(int id);

        bool PetExists(string id);

        bool SearchPostTypeExists(string name);

        bool UserOwnsSearchPost(string searchPostId, string userId);

        bool Save(string searchPostId, string userId);

        bool Remove(string searchPostId, string userId);

        string GetUserId(string id);

        List<LatestSearchPostsServiceModel> Latest();
    }
}
