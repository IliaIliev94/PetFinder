using PetFinder.Data;
using PetFinder.Models.Shared;
using PetFinder.Services.SearchPosts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetFinder.Data.Models;
using PetFinder.Services.Pets;

namespace PetFinder.Services.SearchPosts
{

    public class SearchPostService : ISearchPostService
    {

        private readonly ApplicationDbContext context;
        private readonly IPetService petService;

        public SearchPostService(ApplicationDbContext context, IPetService petService)
        {
            this.context = context;
            this.petService = petService;
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
                IsFound = searchPostType == "Found" ? true : false,
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
            var searchPostQuery = this.context.SearchPosts.AsQueryable();

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

        public IEnumerable<CityCategoryServiceModel> GetCities()
        {
            return this.context.Cities.Select(city => new CityCategoryServiceModel { Id = city.Id, Name = city.Name }).ToList();
        }

        public  IEnumerable<PetSelectServiceModel> GetPets()
        {
            return this.context.Pets.Select(pet => new PetSelectServiceModel { Id = pet.Id, Name = pet.Name }).ToList();
        }

        private string CreatePet(string type, string name, string imageUrl, int speciesId, int sizeId, int? ownerId)
        {
            return petService.Create(name, imageUrl, speciesId, sizeId, ownerId);
        }

        public SearchPostDetailsServiceModel Details(string id)
        {
            return this.context
                .SearchPosts
                .Where(searchPost => searchPost.Id == id)
                .Select(searchPost => new SearchPostDetailsServiceModel
                {
                    Id = searchPost.Id,
                    Title = searchPost.Title,
                    Description = searchPost.Description,
                    ImageUrl = searchPost.Pet.ImageUrl,
                    City = searchPost.City.Name,
                    PetName = searchPost.Pet.Name,
                    PetSpecies = searchPost.Pet.Species.Name,
                })
                .FirstOrDefault();
        }

        public SearchPostEditServiceModel GetEditData(string id)
        {
            return this.context
                .SearchPosts
                .Where(searchPost => searchPost.Id == id)
                .Select(searchPost => new SearchPostEditServiceModel
                {
                    Title = searchPost.Title,
                    Description = searchPost.Description,
                    Type = searchPost.SearchPostType.Name,
                    DateLostFound = searchPost.DateLostFound,
                    CityId = searchPost.CityId,
                    PetId = searchPost.PetId,
                    UserId = searchPost.UserId,
                })
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
            searchPost.CityId = cityId;
            searchPost.DateLostFound = dateLostFound;
            searchPost.PetId = petId;

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
                .Select(searchPost => new SearchPostServiceModel
                {
                    Id = searchPost.Id,
                    Title = searchPost.Title,
                    PetName = searchPost.Pet.Name,
                    PetSpecies = searchPost.Pet.Species.Name,
                    ImageUrl = searchPost.Pet.ImageUrl,
                })
                .ToList();
        }
    }
}
