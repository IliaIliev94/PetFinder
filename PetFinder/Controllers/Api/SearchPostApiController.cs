﻿using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Models.Api.SearchPosts;
using PetFinder.Models.Shared;
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
        private readonly ApplicationDbContext context;

        public SearchPostApiController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<AllSearchPostsApiResponseModel> All([FromQuery]AllSearchPostsApiRequestModel query)
        {
            var searchPostQuery = this.context.SearchPosts.AsQueryable();


            if (!string.IsNullOrWhiteSpace(query.Species))
            {
                searchPostQuery = searchPostQuery.Where(searchPost => searchPost.Pet.Species.Name == query.Species);
            }

            if (!string.IsNullOrWhiteSpace(query.Size))
            {
                searchPostQuery = searchPostQuery.Where(searchPost => searchPost.Pet.Size.Type == query.Size);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTermInvariant = query.SearchTerm.ToLower();

                searchPostQuery = searchPostQuery.Where(searchPost =>
                    searchPost.Title.ToLower().Contains(searchTermInvariant)
                    || searchPost.Description.ToLower().Contains(searchTermInvariant)
                    || searchPost.Pet.Name.ToLower().Contains(searchTermInvariant)
                    || searchPost.Pet.Species.Name.Contains(searchTermInvariant));
            }

            if(!string.IsNullOrWhiteSpace(query.Type))
            {
                searchPostQuery = searchPostQuery.Where(searchPost => searchPost.SearchPostType.Name.ToLower() == query.Type.ToLower());
            }

            searchPostQuery = query.Sorting switch
            {
                SearchPostSorting.DatePublished => searchPostQuery.OrderByDescending(searchPost => searchPost.DatePublished),
                SearchPostSorting.DateLostFound => searchPostQuery.OrderByDescending(searchPost => searchPost.DateLostFound),
                SearchPostSorting.PetSpecies => searchPostQuery.OrderByDescending(searchPost => searchPost.Pet.Species.Id),
                SearchPostSorting.PetSize => searchPostQuery.OrderByDescending(searchPost => searchPost.Pet.Size.Id),
                _ => searchPostQuery.OrderByDescending(searchPost => searchPost.Id),
            };

            var petSizes = this.context.Sizes
                .OrderByDescending(size => size.Id)
                .Select(size => size.Type)
                .ToList();

            var petSpecies = this.context.Species
                .OrderBy(species => species.Id)
                .Select(species => species.Name)
                .Reverse()
                .ToList();

            var searchPosts = searchPostQuery
                .Select(searchPost => new SearchPostResponseModel
                {
                    Id = searchPost.Id,
                    Title = searchPost.Title,
                    ImageUrl = searchPost.Pet.ImageUrl,
                    PetName = searchPost.Pet.Name,
                    PetSpecies = searchPost.Pet.Species.Name,
                })
                .ToList();

            return new AllSearchPostsApiResponseModel { SearchPosts = searchPosts };
        }
        
    }
}
