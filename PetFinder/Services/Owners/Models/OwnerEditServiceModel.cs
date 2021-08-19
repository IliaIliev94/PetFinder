using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Owners.Models
{
    public class OwnerEditServiceModel
    {
        public int? Id { get; init; }
        public string Name { get; init; }
        public string PhoneNumber { get; set; }
    }
}
