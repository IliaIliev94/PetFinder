using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class Pet
    {
        public Pet()
        {
            this.SearchPosts = new HashSet<SearchPost>();
        }
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public Size Size { get; set; }

        public int SpeciesId { get; set; }

        public virtual Species Species { get; set; }

        public virtual ICollection<SearchPost> SearchPosts { get; set; }


    }
}
