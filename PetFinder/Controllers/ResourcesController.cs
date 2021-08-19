using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Infrastructure;
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

        public IActionResult All()
        {
            var resources = this.resourcesService.All();

            return this.View(resources);
        }

        

        public IActionResult Details(string id)
        {
            var resourcePost = this.resourcesService.Details(id);

            return this.View(resourcePost);
        }

       
    }
}
