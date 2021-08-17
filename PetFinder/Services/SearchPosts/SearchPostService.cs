using PetFinder.Data;
using PetFinder.Models.Shared;
using PetFinder.Services.SearchPosts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetFinder.Data.Models;
using PetFinder.Services.Pets;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace PetFinder.Services.SearchPosts
{

    public class SearchPostService : ISearchPostService
    {

        private readonly ApplicationDbContext context;
        private readonly IPetService petService;
        private readonly IMapper mapper;

        public SearchPostService(ApplicationDbContext context, IPetService petService, IMapper mapper)
        {
            this.context = context;
            this.petService = petService;
            this.mapper = mapper;
        }

        public string Create(
            string title,
            string description,
            string searchPostType,
            int cityId,
            DateTime? dateLostFound,
            string petId,
            string petName,
            string imageUrl,
            int speciesId,
            int sizeId,
            int? ownerId,
            string userId)
        {
            var newSearchPost = new SearchPost
            {
                Title = title,
                Description = description,
                IsFoundClaimed = false,
                CityId = cityId,
                DatePublished = DateTime.UtcNow,
                DateLostFound = dateLostFound,
                SearchPostTypeId = searchPostType == "Found" ? 1 : 2,
                PetId = (searchPostType == "Found" || petId == "0") ? CreatePet(searchPostType, petName, imageUrl, speciesId, sizeId, ownerId) : petId,
                UserId = userId,
            };



            this.context.SearchPosts.Add(newSearchPost);

            this.context.SaveChanges();

            return newSearchPost.Id;
        }

        public SearchPostQueryServiceModel All(string species,
            string size,
            string searchTerm,
            string city,
            string type,
            int currentPage,
            int searchPostsPerPage,
            SearchPostSorting sorting)
        {
            var searchPostQuery = this.context.SearchPosts.Where(searchPost => !searchPost.Is).AsQueryable();

            var totalPages = (int)Math.Ceiling(this.context.SearchPosts.Where(searchPost => searchPost.SearchPostType.Name == type).Count() * 1.0 / searchPostsPerPage);

            if(currentPage < 1)
            {
                currentPage = 1;
            }

            if(currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            if (!string.IsNullOrWhiteSpace(species))
            {
                searchPostQuery = searchPostQuery.Where(searchPost => searchPost.Pet.Species.Name == species);
            }

            if (!string.IsNullOrWhiteSpace(size))
            {
                searchPostQuery = searchPostQuery.Where(searchPost => searchPost.Pet.Size.Type == size);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermInvariant = searchTerm.ToLower();

                searchPostQuery = searchPostQuery.Where(searchPost =>
                    searchPost.Title.ToLower().Contains(searchTermInvariant)
                    || searchPost.Description.ToLower().Contains(searchTermInvariant)
                    || searchPost.Pet.Name.ToLower().Contains(searchTermInvariant)
                    || searchPost.Pet.Species.Name.Contains(searchTermInvariant));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                searchPostQuery = searchPostQuery.Where(searchPost => searchPost.SearchPostType.Name.ToLower() == type.ToLower());
            }

            if(!string.IsNullOrWhiteSpace(city))
            {
                searchPostQuery = searchPostQuery.Where(searchPost => searchPost.City.Name == city);
            }

            searchPostQuery = sorting switch
            {
                SearchPostSorting.DatePublished => searchPostQuery.OrderByDescending(searchPost => searchPost.DatePublished),
                SearchPostSorting.DateLostFound => searchPostQuery.OrderByDescending(searchPost => searchPost.DateLostFound),
                SearchPostSorting.PetSpecies => searchPostQuery.OrderByDescending(searchPost => searchPost.Pet.Species.Id),
                SearchPostSorting.PetSize => searchPostQuery.OrderByDescending(searchPost => searchPost.Pet.Size.Id),
                SearchPostSorting.City => searchPostQuery.OrderByDescending(searchPost => searchPost.City.Name),
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

            var cities = this.context.Cities
                .OrderBy(city => city.Id)
                .Select(city => city.Name)
                .ToList();

            var parsedPageNumber = currentPage - 1;

            var searchPosts = searchPostQuery
                .Skip((parsedPageNumber > 0 ? parsedPageNumber : 0) * searchPostsPerPage)
                .Take(searchPostsPerPage)
                .Select(searchPost => new SearchPostServiceModel
                {
                    Id = searchPost.Id,
                    Title = searchPost.Title,
                    ImageUrl = searchPost.Pet.ImageUrl,
                    PetName = searchPost.Pet.Name,
                    PetSpecies = searchPost.Pet.Species.Name,
                })
                .ToList();

            return new SearchPostQueryServiceModel { TotalPages = totalPages, CurrentPage = currentPage, PetSizes = petSizes, PetSpecies = petSpecies, SearchPosts = searchPosts, Cities = cities, };
        }

        public SearchPostDetailsServiceModel Details(string id)
        {
            return this.context
                .SearchPosts
                .Where(searchPost => searchPost.Id == id)
                .ProjectTo<SearchPostDetailsServiceModel>(mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public SearchPostEditServiceModel GetEditData(string id)
        {
            return this.context
                .SearchPosts
                .Where(searchPost => searchPost.Id == id)
                .ProjectTo<SearchPostEditServiceModel>(mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public bool Edit(string id, string title, string description, int cityId, DateTime? dateLostFound, string petId)
        {
            var searchPost = this.context.SearchPosts.FirstOrDefault(searchPost => searchPost.Id == id);

            if(searchPost == null)
            {
                return false;
            }

            searchPost.Title = title;
            searchPost.Description = description;
            searchPost.DateLostFound = dateLostFound;
            if(cityId != 0)
            {
                searchPost.CityId = cityId;
            }
            if(!string.IsNullOrWhiteSpace(petId))
            {
                searchPost.PetId = petId;
            }
            

            this.context.SaveChanges();

            return true;

        }

        public bool Edit(string id, string title, string description, int cityId, DateTime? dateLostFound, string petName, string imageUrl, int petSpeciesId, int petSizeId)
        {
           var petId = this.context.SearchPosts.FirstOrDefault(searchPost => searchPost.Id == id).PetId;
           var isSearchPostEditSuccessfull = this.Edit(id, title, description, cityId, dateLostFound, petId);
           var isPetEditSuccessfull = this.petService.Edit(petId, petName, imageUrl, petSpeciesId, petSizeId);

            if(!isSearchPostEditSuccessfull || !isPetEditSuccessfull)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<SearchPostServiceModel> GetSearchPostsById(string id)
        {
            return this.context.SearchPosts
                .Where(searchPost => searchPost.UserId == id)
                .ProjectTo<SearchPostServiceModel>(mapper.ConfigurationProvider)
                .ToList();
        }

        public Tuple<bool, string> Delete(string id, string userId, bool userIsAdmin)
        {
            var searchPost = this.context.SearchPosts.Include(searchPost => searchPost.SearchPostType).FirstOrDefault(searchPost => searchPost.Id == id);
            var type = searchPost.SearchPostType.Name;

            if(searchPost == null)
            {
                return Tuple.Create(false, "");
            }

            if(searchPost.UserId != userId && !userIsAdmin)
            {
                return Tuple.Create(false, "");
            }

            var petId = String.Empty;

            if(type == "Found")
            {
                petId = searchPost.PetId;
            }

            this.context.SearchPosts.Remove(searchPost);

            if (!string.IsNullOrEmpty(petId))
            {
                this.context.Pets.Remove(this.context.Pets.FirstOrDefault(pet => pet.Id == petId));
            }

            this.context.SaveChanges();

            return Tuple.Create(true, type);
        }

        public string SetAsFoundClaimed(string id)
        {
            var searchPost = this.context
                .SearchPosts
                .FirstOrDefault(searchPost => searchPost.Id == id);

            searchPost.IsFoundClaimed = true;

            return searchPost.SearchPostType.Name;
        }

        public IEnumerable<CityCategoryServiceModel> GetCities()
        {
            return this.context.Cities
                .ProjectTo<CityCategoryServiceModel>(mapper.ConfigurationProvider).ToList();
        }

        public IEnumerable<PetSelectServiceModel> GetPets(int? ownerId)
        {
            return this.context.Pets
                .Where(searchPost => searchPost.OwnerId == ownerId)
                .ProjectTo<PetSelectServiceModel>(mapper.ConfigurationProvider)
                .ToList();
        }

        public bool CityExists(int id)
        {
            return this.context.Cities
                .Any(city => city.Id == id);
        }

        public bool PetExists(string id)
        {
            return this.context.Pets
                .Any(pet => pet.Id == id);
        }

        public string GetUserId(string id)
        {
            return this.context
                .SearchPosts
                .FirstOrDefault(searchPost => searchPost.Id == id)
                .UserId;
        }

        public List<LatestSearchPostsServiceModel> Latest()
        {
            return this.context
                .SearchPosts
                .OrderByDescending(searchPost => searchPost.DatePublished)
                .ProjectTo<LatestSearchPostsServiceModel>(mapper.ConfigurationProvider)
                .Take(3)
                .ToList();
        }

        public bool UserOwnsSearchPost(string searchPostId, string userId)
        {
            return this.context
                .SearchPosts
                .Any(searchPost => searchPost.Id == searchPostId && searchPost.UserId == userId);
        }


        private string CreatePet(string type, string name, string imageUrl, int speciesId, int sizeId, int? ownerId)
        {
            return petService.Create(name, imageUrl, speciesId, sizeId, ownerId);
        }
    }
}
