using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class Comment
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string Content { get; set; }

        public DateTime CreatedOn { get; init; }

        public string SearchPostId { get; set; }

        public virtual SearchPost SearchPost { get; set; }

        public string ResourcePostId { get; set; }

        public virtual ResourcePost ResourcePost { get; set; }
    }
}
