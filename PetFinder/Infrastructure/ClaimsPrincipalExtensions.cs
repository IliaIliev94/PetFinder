using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using static PetFinder.WebConstantscs;

namespace PetFinder.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdministratorRoleName);
        }
    }
}
