using PetFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFinder.Tests.Data
{
    public static class SearchPostsData
    {
        public static IEnumerable<SearchPost> GetLostSearchPosts()
        {
            return Enumerable.Range(0, 10).Select(i => new SearchPost { Id = i.ToString(), Title = i.ToString(), SearchPostType = new SearchPostType { Name = "Lost"} });
        }

        public static IEnumerable<SearchPost> GetFoundSearchPosts()
        {
            return Enumerable.Range(0, 10).Select(i => new SearchPost { Id = i.ToString(), Title = i.ToString(), SearchPostType = new SearchPostType { Name = "Found" } });
        }

        public static SearchPost GetSearchPost(string id, string title, string description, string petId, int cityId, string userId)
        {
            return new SearchPost { Id = id, Title = title, Description = description, PetId = petId, CityId = cityId, UserId = userId, SearchPostType = new SearchPostType { Id = 1, Name = "Test"} };
        }

    }
}
