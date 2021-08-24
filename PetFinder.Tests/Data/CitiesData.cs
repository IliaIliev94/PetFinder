using PetFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFinder.Tests.Data
{
    public static class CitiesData
    {
        public static City GetCity()
        {
            return new City { Id = 1, Name = "Test"};
        }

        public static IEnumerable<City> GetCities()
        {
            return Enumerable.Range(1, 10).Select(i => new City { Id = i, Name = i.ToString()});
        }
    }
}
