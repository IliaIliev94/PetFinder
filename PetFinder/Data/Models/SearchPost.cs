using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class SearchPost
    {
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsFound { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DatePublished { get; init; }

        public DateTime? DateLostFound { get; set; }

        public virtual Pet Pet { get; set; }

        public virtual SearchPostType Type { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        
    }
}
