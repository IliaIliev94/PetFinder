using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Data.Models
{
    public class UserSearchPost
    {
        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }

        public string SearchPostId { get; set; }

        public virtual SearchPost SearchPost { get; set; }
    }
}
