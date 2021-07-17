using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class City
    {
        public City()
        {
            this.Pets = new HashSet<Pet>();
        }

        public int Id { get; init; }

        public string Name { get; init; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
