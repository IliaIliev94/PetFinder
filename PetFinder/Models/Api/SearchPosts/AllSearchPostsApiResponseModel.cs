﻿using PetFinder.Models.SearchPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Models.Api.SearchPosts
{
    public class AllSearchPostsApiResponseModel
    {
        public IEnumerable<SearchPostResponseModel> SearchPosts { get; set; }
    }
}
