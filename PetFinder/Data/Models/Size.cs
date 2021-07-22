using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class Size
    {
        public Size()
        {
            this.Pets = new HashSet<Pet>();
        }
        public int Id { get; init; }

        public string Type { get; init; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
