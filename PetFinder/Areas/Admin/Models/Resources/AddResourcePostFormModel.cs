using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using static PetFinder.Data.DataConstraints.ResourcePost;

namespace PetFinder.Areas.Admin.Models.Resources
{
    public class AddResourcePostFormModel
    {

        public string Id { get; init; }

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; init; }

        [Required]
        [MinLength(MinDescriptionLength)]
        public string Description { get; init; }

        [Required]
        public string ImageUrl { get; init; }

    }
}
