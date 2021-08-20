using PetFinder.Services.Owners.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Owners
{
    public interface IOwnerService
    {

        bool IsOwner(string userId);

        int? GetOwnerId(string userId);

        string GetPhoneNumber(string userId);

        void Add(string name, string phoneNumber, string userId);

        OwnerEditServiceModel GetOwnerData(string userId);

        void Edit(string userId, string name, string phoneNumber);

    }
}
