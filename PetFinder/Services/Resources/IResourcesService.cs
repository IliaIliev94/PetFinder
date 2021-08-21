using PetFinder.Services.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Resources
{
    public interface IResourcesService
    {
        ResourcePostQueryServiceModel All(string searchTerm, int currentPage, int resourcePostsPerPage);

        ResourcePostDetailsServiceModel Details (string id);

        ResourcePostEditServiceModel GetEditData(string id);

        string Create(string title, string description, string imageUrl);

        bool Edit(string id, string title, string description, string imageUrl);

        bool Delete(string id);

        bool ResourcePostExists(string id);
    }
}
