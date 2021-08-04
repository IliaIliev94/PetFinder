﻿using PetFinder.Data;
using PetFinder.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.SearchPosts
{

    public class SearchPostService : ISearchPostService
    {

        private readonly ApplicationDbContext context;

        public SearchPostService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public SearchPostQueryServiceModel All(string species,
            string size,
            string searchTerm,
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

            searchPostQuery = sorting switch
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
                .Skip((currentPage - 1) * searchPostsPerPage)
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

            return new SearchPostQueryServiceModel { TotalPages = totalPages, CurrentPage = currentPage, PetSizes = petSizes, PetSpecies = petSpecies, SearchPosts = searchPosts };
        }
    }
}