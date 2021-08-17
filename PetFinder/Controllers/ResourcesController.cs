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
        private readonly IMapper mapper;
        private readonly IResourcesService resourcesService;

        public ResourcesController(IResourcesService resourcesService, IMapper mapper)
        {
            this.resourcesService = resourcesService;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            var resources = this.resourcesService.All();

            return this.View(resources);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Add(AddResourcePostFormModel resourcePost)
        {

            if(!this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            var id = this.resourcesService.Create(resourcePost.Title, resourcePost.Description, resourcePost.ImageUrl);

            return this.RedirectToAction("Details", new { Id = id });
        }

        public IActionResult Details(string id)
        {
            var resourcePost = this.resourcesService.Details(id);

            return this.View(resourcePost);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Edit(string id)
        {

            if(!this.resourcesService.ResourcePostExists(id))
            {
                return this.NotFound();
            }

            var resourcePost = this.mapper.Map<AddResourcePostFormModel>(this.resourcesService.GetEditData(id));

            return this.View(resourcePost);
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Edit(AddResourcePostFormModel resource)
        {

            if (!this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            if(!ModelState.IsValid)
            {
                return this.View(resource);
            }

            var isEditSuccessfull = this.resourcesService.Edit(resource.Id, resource.Title, resource.Description, resource.ImageUrl);

            if(!isEditSuccessfull)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("Details", new {Id = resource.Id});
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Delete(string id)
        {

            if(!this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            var isDeleteSuccessfull = this.resourcesService.Delete(id);

            if(!isDeleteSuccessfull)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All");
        }
    }
}
