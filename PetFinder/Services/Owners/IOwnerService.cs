using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Owners
{
    public interface IOwnerService
    {

        bool IsOwner(string userId);

        int GetOwnerId(string userId);

    }
}
