using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class SearchPost
    {
        public SearchPost()
        {
            this.Comments = new HashSet<Comment>();
        }

        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Description { get; set; }

        public bool IsFoundClaimed { get; set; }

        public DateTime DatePublished { get; init; }

        public DateTime? DateLostFound { get; set; }

        public string PetId { get; set; }

        public virtual Pet Pet { get; set; }

        public int SearchPostTypeId { get; set; }

        public virtual SearchPostType SearchPostType { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }


    }
}
