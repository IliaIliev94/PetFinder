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
    }
}
