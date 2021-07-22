using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class SearchPostType
    {
        public SearchPostType()
        {
            this.SearchPosts = new HashSet<SearchPost>();
        }
        public int Id { get; init; }

        public string Name { get; init; }

        public virtual ICollection<SearchPost> SearchPosts { get; set; }
    }
}
