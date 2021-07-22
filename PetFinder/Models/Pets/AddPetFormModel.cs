﻿using PetFinder.Models.Sizes;
using PetFinder.Models.Species;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using static PetFinder.Data.DataConstraints.Pet;

namespace PetFinder.Models.Pets
{
    public class AddPetFormModel
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        [Display(Name ="Image URL")]
        public string ImageUrl { get; init; }

        public int SizeId { get; init; }

        public ICollection<SizeViewModel> Sizes { get; set; }

        public int SpeciesId { get; init; }

        public ICollection<SpeciesViewModel> Species { get; set; }
    }
}
