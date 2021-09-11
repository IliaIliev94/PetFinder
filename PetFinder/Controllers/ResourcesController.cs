using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Infrastructure;
using PetFinder.Models.Resources;
using PetFinder.Services.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Controllers
{
    public class ResourcesController : Controller
    {

        private readonly IResourcesService resourcesService;

        public ResourcesController(IResourcesService resourcesService)
        {
            this.resourcesService = resourcesService;
        }

        public IActionResult All([FromQuery] AllResourcePostsViewModel query, int currentPage)
        {
            query.Pagination.CurrentPage = currentPage != 0 ? currentPage : 1;
            var queryResult = this.resourcesService.All(query.SearchTerm, query.Pagination.CurrentPage, query.Pagination.PostsPerPage);
            query.ResourcePosts = queryResult.Resources;
            query.Pagination.TotalPages = queryResult.TotalPages;
            query.Pagination.CurrentPage = queryResult.CurrentPage;
            return this.View(query);
        }

        

        public IActionResult Details([FromQuery] ResourcesDetailsViewModel query, string id, int currentPage = 1)
        {
            var queryResult = this.resourcesService.Details(id, currentPage, query.Pagination.PostsPerPage);

            query.ResourcePost = queryResult.ResourcePost;
            query.Pagination.CurrentPage = queryResult.CurrentPage;
            query.Pagination.TotalPages = queryResult.TotalPages;

            return this.View(query);
        }

       
    }
}
