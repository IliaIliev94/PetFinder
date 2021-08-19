using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using static PetFinder.Data.DataConstraints.Owner;

namespace PetFinder.Models.Owners
{
    public class BecomeOwnerFormModel
    {

        public int? Id { get; init; }
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(PhoneMaxLength, MinimumLength = PhoneMinLength)]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; init; }
    }
}
