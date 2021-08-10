using PetFinder.Services.Pets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.SearchPosts.Models
{
    public class SearchPostEditServiceModel
    {
        public string Title { get; init; }

        public string Description { get; init; }

        public string Type { get; init; }

        public DateTime? DateLostFound { get; init; }

        public string SearchPostType { get; init; }

        public string PetId { get; init; }

        public IEnumerable<PetSelectServiceModel> Pets { get; set; }

        public int CityId { get; init; }

        public IEnumerable<CityCategoryServiceModel> Cities { get; set; }

        public PetEditServiceModel Pet { get; set; }

        public string UserId { get; init; }
    }
}
