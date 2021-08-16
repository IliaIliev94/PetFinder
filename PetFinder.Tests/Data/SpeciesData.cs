using PetFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFinder.Tests.Data
{
    public static class SpeciesData
    {
        public static Specie GetSpecies()
        {
            return new Specie { Id = 2, Name = "Test" };
        }
    }
}
