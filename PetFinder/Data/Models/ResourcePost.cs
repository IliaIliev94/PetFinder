using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class ResourcePost
    {
        public ResourcePost()
        {
            this.Comments = new HashSet<Comment>();
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; }

        public string Descripton { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
