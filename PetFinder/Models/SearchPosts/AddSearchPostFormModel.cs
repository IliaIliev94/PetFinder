using PetFinder.Models.Pets;
using PetFinder.Services.SearchPosts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


using static PetFinder.Data.DataConstraints.SearchPost;

namespace PetFinder.Models.SearchPosts
{
    public class AddSearchPostFormModel
    {

        public string Id { get; set; }

        [Required]
        [StringLength(MaxTitleLength, MinimumLength = MinTitleLength)]
        public string Title { get; init; }

        [Required]
        [MinLength(MinDescriptionLength)]
        public string Description { get; init; }

        public DateTime DatePublished { get; init; }

        public DateTime? DateLostFound { get; init; }

        public string SearchPostType { get; init; }

        public string PetId { get; init; }

        public IEnumerable<PetSelectServiceModel> Pets { get; set; }

        public int CityId { get; init; }

        public IEnumerable<CityCategoryServiceModel> Cities { get; set; }

        public AddPetFormModel Pet { get; set; }

        public string UserId { get; set; }
    }
}
