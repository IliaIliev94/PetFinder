using PetFinder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Owners
{
    public class OwnerService : IOwnerService
    {

        private readonly ApplicationDbContext context;

        public OwnerService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool IsOwner(string userId)
        {
            return this.context.Owners.Any(owner => owner.UserId == userId);
        }

        public int GetOwnerId(string userId)
        {
            return this.context.Owners.FirstOrDefault(owner => owner.UserId == userId).Id;
        }

    }
}
