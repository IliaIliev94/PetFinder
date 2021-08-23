using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Models.Api.SearchPosts;
using PetFinder.Models.Shared;
using PetFinder.Services.SearchPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Controllers.Api
{
    [ApiController]
    [Route("api/searchposts")]
    public class SearchPostApiController : ControllerBase
    {
        private readonly ISearchPostService searchPostService;

        public SearchPostApiController(ISearchPostService searchPostService)
        {
            this.searchPostService = searchPostService;
        }

        [HttpGet]
        public ActionResult<AllSearchPostsApiResponseModel> All([FromQuery]AllSearchPostsApiRequestModel query)
        {

           var queryResult =  this.searchPostService.All(
                query.Species,
                query.Size,
                query.SearchTerm,
                query.City,
                query.Type,
                query.CurrentPage,
                AllSearchPostsApiRequestModel.PostsPerPage,
                query.Sorting);

            return new AllSearchPostsApiResponseModel { SearchPosts = queryResult.SearchPosts };
        }
        
    }
}
