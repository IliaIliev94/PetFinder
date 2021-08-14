using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using static PetFinder.Data.DataConstraints.ResourcePost;

namespace PetFinder.Data.Models
{
    public class ResourcePost
    {
        public ResourcePost()
        {
            this.Comments = new HashSet<Comment>();
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(MinDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
