﻿using System;
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
        }

        public class Pet
        {
            public const int NameMaxLength = 50;
        }

        public class Owner
        {
            public const int NameMaxLength = 40;
            public const int PhoneMinLength = 6;
            public const int PhoneMaxLength = 30;
        }
    }
}
