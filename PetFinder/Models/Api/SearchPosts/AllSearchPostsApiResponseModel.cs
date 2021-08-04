using PetFinder.Models.SearchPosts;
using PetFinder.Services.SearchPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Api.SearchPosts
{
    public class AllSearchPostsApiResponseModel
    {
        public IEnumerable<SearchPostServiceModel> SearchPosts { get; set; }
    }
}
