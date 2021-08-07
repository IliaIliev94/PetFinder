using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using static PetFinder.Data.DataConstraints.Pet;

namespace PetFinder.Data.Models
{
    public class Pet
    {
        public Pet()
        {
            this.SearchPosts = new HashSet<SearchPost>();
        }

        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int SizeId { get; set; }

        public Size Size { get; set; }

        public int SpeciesId { get; set; }

        public int? OwnerId { get; init; }

        public Owner Owner { get; init; }

        public virtual Specie Species { get; set; }

        public virtual ICollection<SearchPost> SearchPosts { get; set; }


    }
}
