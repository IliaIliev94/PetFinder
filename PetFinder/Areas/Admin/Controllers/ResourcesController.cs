using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Areas.Admin.Models.Resources;
using PetFinder.Infrastructure;
using PetFinder.Services.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Areas.Admin.Controllers
{
    public class ResourcesController : AdminController
    {
        private readonly IResourcesService resourcesService;
        private readonly IMapper mapper;

        public ResourcesController(IResourcesService resourcesService, IMapper mapper)
        {
            this.resourcesService = resourcesService;
            this.mapper = mapper;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Add(AddResourcePostFormModel resourcePost)
        {

            if (!this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            var id = this.resourcesService.Create(resourcePost.Title, resourcePost.Description, resourcePost.ImageUrl);

            return this.RedirectToAction("Details", "Resources", new { Id = id, Area = "" });
        }

        public IActionResult Edit(string id)
        {

            if (!this.resourcesService.ResourcePostExists(id))
            {
                return this.NotFound();
            }

            var resourcePost = this.mapper.Map<AddResourcePostFormModel>(this.resourcesService.GetEditData(id));

            return this.View(resourcePost);
        }

        [HttpPost]
        public IActionResult Edit(AddResourcePostFormModel resource)
        {

            if (!this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return this.View(resource);
            }

            var isEditSuccessfull = this.resourcesService.Edit(resource.Id, resource.Title, resource.Description, resource.ImageUrl);

            if (!isEditSuccessfull)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("Details", new { Id = resource.Id, Area = "" });
        }

        public IActionResult Delete(string id)
        {

            if (!this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            var isDeleteSuccessfull = this.resourcesService.Delete(id);

            if (!isDeleteSuccessfull)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All", new { Area = "" });
        }
    }
}
