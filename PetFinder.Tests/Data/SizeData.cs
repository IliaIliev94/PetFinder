using PetFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFinder.Tests.Data
{
    public static class SizeData
    {
        public static Size GetSize()
        {
            return new Size { Id = 1, Type = "Test" };
        }
    }
}
