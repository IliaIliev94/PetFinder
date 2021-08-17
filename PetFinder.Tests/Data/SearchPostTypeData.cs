using PetFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFinder.Tests.Data
{
    public static class SearchPostTypeData
    {
        public static IEnumerable<SearchPostType> GetSearchPostTypes()
        {
            return new List<SearchPostType>()
            {
                new SearchPostType { Id = 1, Name = "Lost"},
                new SearchPostType { Id = 2, Name = "Found"},
            };
        }
    }
}
