﻿using Microsoft.AspNetCore.Mvc;
using PetFinder.Data;
using PetFinder.Models.Pets;
using PetFinder.Models.Sizes;
using PetFinder.Models.Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetFinder.Data.Models;

namespace PetFinder.Controllers
{
    public class PetsController : Controller
    {
        private ApplicationDbContext context;

        public PetsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult All()
        {
            var pets = this.context
                .Pets
                .Select(pet => new PetListViewModel
                {
                    Id = pet.Id,
                    Name = pet.Name,
                    ImageUrl = pet.ImageUrl,
                    Species = pet.Species.Name,
                    Size = pet.Size.Type,
                })
                .ToList();

            return this.View(pets);
        }

        public IActionResult Add(string searchId)
        {

            ViewBag.SearchId = searchId;
           
            return this.View(new AddPetFormModel 
            { 
                Sizes = this.GetSizes(),
                Species = this.GetSpecies(),
            });
        }

        [HttpPost]
        public IActionResult Add(AddPetFormModel pet)
        {
            if(!ModelState.IsValid)
            {
                pet.Sizes = this.GetSizes();
                pet.Species = this.GetSpecies();

                return this.View(pet);
            }

            var newPet = new Pet
            {
                Name = pet.Name,
                ImageUrl = pet.ImageUrl,
                SizeId = pet.SizeId,
                SpeciesId = pet.SpeciesId,
            };

            if(!String.IsNullOrEmpty(pet.SearchPostId))
            {
                this.context.SearchPosts.FirstOrDefault(searchPost => searchPost.Id == pet.SearchPostId).PetId = newPet.Id;
            }

            

            this.context.Pets.Add(newPet);

            this.context.SaveChanges();

            return this.RedirectToAction("All", "SearchPosts");
        }

        private ICollection<SizeViewModel> GetSizes()
        {
            return this.context.Sizes.Select(size => new SizeViewModel { Id = size.Id, Type = size.Type }).ToList();
        }

        private ICollection<SpeciesViewModel> GetSpecies()
        {
            var speciesList = this.context.Species.Select(specie => new SpeciesViewModel { Id = specie.Id, Name = specie.Name }).ToList();
            speciesList.Reverse();

            return speciesList;
        }
    }
}
