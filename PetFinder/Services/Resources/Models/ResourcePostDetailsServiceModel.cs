using PetFinder.Services.Comments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Resources.Models
{
    public class ResourcePostDetailsServiceModel
    {
        public string Id { get; set; }

        public string Title { get; init; }

        public string Description  { get; init; }

        public string ImageUrl { get; init; }

        public DateTime CreatedOn { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; init; }

    }
}
