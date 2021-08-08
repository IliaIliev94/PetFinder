using PetFinder.Models.Cities;
using PetFinder.Models.Pets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using static PetFinder.Data.DataConstraints.SearchPost;

namespace PetFinder.Models.SearchPosts
{
    public class AddSearchPostFormModel
    {
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

        public IEnumerable<PetListViewModel> Pets { get; set; }

        public int CityId { get; init; }

        public IEnumerable<CityViewModel> Cities { get; set; }

        public AddPetFormModel Pet { get; set; }
    }
}
