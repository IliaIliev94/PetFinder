using AutoMapper;
using AutoMapper.QueryableExtensions;
using PetFinder.Data;
using PetFinder.Services.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetFinder.Data.Models;

namespace PetFinder.Services.Resources
{
    public class ResourcesService : IResourcesService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ResourcesService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<ResourcePostServiceModel> All()
        {
            return this.context
                .ResourcePosts
                .OrderByDescending(resourcePost => resourcePost.CreatedOn)
                .ProjectTo<ResourcePostServiceModel>(mapper.ConfigurationProvider)
                .ToList();
        }

        public ResourcePostDetailsServiceModel Details(string id)
        {
            return this.context
                .ResourcePosts
                .Where(resourcePost => resourcePost.Id == id)
                .ProjectTo<ResourcePostDetailsServiceModel>(mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public ResourcePostEditServiceModel GetEditData(string id)
        {
            return this.context
                .ResourcePosts
                .Where(resourcePost => resourcePost.Id == id)
                .ProjectTo<ResourcePostEditServiceModel>(mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public string Create(string title, string description, string imageUrl)
        {
            var resourcePost = new ResourcePost
            {
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
                CreatedOn = DateTime.UtcNow,
            };

            this.context.ResourcePosts.Add(resourcePost);

            this.context.SaveChanges();

            return resourcePost.Id;
        }


        public bool Edit(string id, string title, string description, string imageUrl)
        {
            var resourcePost = this.context
                .ResourcePosts
                .FirstOrDefault(resourcePost => resourcePost.Id == id);

            if(resourcePost == null)
            {
                return false;
            }

            resourcePost.Title = title;
            resourcePost.Description = description;
            resourcePost.ImageUrl = imageUrl;

            this.context.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var resourcePost = this.context.ResourcePosts.FirstOrDefault(resourcePost => resourcePost.Id == id);

            if(resourcePost == null)
            {
                return false;
            }

            this.context.ResourcePosts.Remove(resourcePost);

            this.context.SaveChanges();

            return true;
        }

        public bool ResourcePostExists(string id)
        {
            return this.context
                .ResourcePosts
                .Any(resourcePost => resourcePost.Id == id);
        }
    }
}
