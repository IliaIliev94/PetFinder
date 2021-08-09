using PetFinder.Services.Pets.Models;
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

        public string Id { get; init; }

        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        [Required]
        [Display(Name ="Image URL")]
        public string ImageUrl { get; init; }

        [Display(Name = "Size")]
        public int SizeId { get; init; }

        public ICollection<SizeCategoryServiceModel> Sizes { get; set; }

        [Display(Name = "Species")]
        public int SpeciesId { get; init; }

        public ICollection<SpeciesCategoryServiceModel> Species { get; set; }

    }
}
