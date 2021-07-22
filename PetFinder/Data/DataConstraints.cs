using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data
{
    public class DataConstraints
    {
        public class SearchPost
        {
            public const int MinTitleLength = 4;
            public const int MaxTitleLength = 50;
            public const int MinDescriptionLength = 10;
            public const int MaxDescriptionLength = 150;
        }

        public class Pet
        {
            public const int NameMaxLength = 50;
        }
    }
}
