﻿using AutoMapper;
using PetFinder.Data.Models;
using PetFinder.Models.Pets;
using PetFinder.Models.Resources;
using PetFinder.Models.SearchPosts;
using PetFinder.Services.Pets.Models;
using PetFinder.Services.Resources.Models;
using PetFinder.Services.SearchPosts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<SearchPostEditServiceModel, AddSearchPostFormModel>();
            this.CreateMap<PetEditServiceModel, AddPetFormModel>();

            this.CreateMap<SearchPost, SearchPostServiceModel>()
                .ForMember(x => x.ImageUrl, y => y.MapFrom(s => s.Pet.ImageUrl))
                .ForMember(x => x.PetName, y => y.MapFrom(s => s.Pet.Name))
                .ForMember(x => x.PetSpecies, y => y.MapFrom(s => s.Pet.Species.Name));

            this.CreateMap<SearchPost, SearchPostDetailsServiceModel>()
                .ForMember(x => x.ImageUrl, y => y.MapFrom(s => s.Pet.ImageUrl))
                .ForMember(x => x.City, y => y.MapFrom(s => s.City.Name))
                .ForMember(x => x.PetSpecies, y => y.MapFrom(s => s.Pet.Species.Name))
                .ForMember(x => x.PetName, y => y.MapFrom(s => s.Pet.Name))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(s => s.Pet.Owner.PhoneNumber));

            this.CreateMap<SearchPost, SearchPostEditServiceModel>()
                .ForMember(x => x.Type, y => y.MapFrom(s => s.SearchPostType.Name))
                .ForMember(x => x.Pet, y => y.Ignore());

            this.CreateMap<Pet, PetServiceModel>()
                .ForMember(x => x.Species, y => y.MapFrom(s => s.Species.Name))
                .ForMember(x => x.Size, y => y.MapFrom(s => s.Size.Type));

            this.CreateMap<Pet, PetDetailsServiceModel>()
                .ForMember(x => x.Species, y => y.MapFrom(s => s.Species.Name))
                .ForMember(x => x.Size, y => y.MapFrom(s => s.Size.Type));

            this.CreateMap<Pet, PetEditServiceModel>();

            this.CreateMap<City, CityCategoryServiceModel>();
            this.CreateMap<Pet, PetSelectServiceModel>();

            this.CreateMap<Size, SizeCategoryServiceModel>();
            this.CreateMap<Specie, SpeciesCategoryServiceModel>();

            this.CreateMap<ResourcePost, ResourcePostServiceModel>()
                .ForMember(x => x.Description, y =>  y.MapFrom(s => s.Description.Substring(0, (Math.Min(100, s.Description.Length)))));

            this.CreateMap<ResourcePost, ResourcePostDetailsServiceModel>();
            this.CreateMap<ResourcePost, ResourcePostEditServiceModel>()
                .ForMember(x => x.Description, y => y.MapFrom(s => s.Description.Trim()));
            this.CreateMap<ResourcePostEditServiceModel, AddResourcePostFormModel>();

        }
    }
}
