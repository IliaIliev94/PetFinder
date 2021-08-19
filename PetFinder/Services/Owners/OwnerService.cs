using AutoMapper;
using PetFinder.Data;
using PetFinder.Data.Models;
using PetFinder.Services.Owners.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Owners
{
    public class OwnerService : IOwnerService
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OwnerService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool IsOwner(string userId)
        {
            return this.context.Owners.Any(owner => owner.UserId == userId);
        }

        public int? GetOwnerId(string userId)
        {
            return this.context.Owners.FirstOrDefault(owner => owner.UserId == userId).Id;
        }

        public string GetPhoneNumber(string userId)
        {
            return this.context.Owners
                .Where(owner => owner.UserId == userId)
                .Select(owner => owner.PhoneNumber)
                .FirstOrDefault();
        }

        public void Add(string name, string phoneNumber, string userId)
        {
            var newOwner = new Owner
            {
                Name = name,
                PhoneNumber = phoneNumber,
                UserId = userId,
            };

            this.context.Owners.Add(newOwner);
            this.context.SaveChanges();
        }

        public OwnerEditServiceModel GetOwnerData(string userId)
        {
            return this.mapper.Map<OwnerEditServiceModel>(this.context.Owners.FirstOrDefault(owner => owner.UserId == userId));
        }

        public void Edit(int? id, string name, string phoneNumber)
        {
            var owner = this.context.Owners.FirstOrDefault(owner => owner.Id == id);
            owner.Name = name;
            owner.PhoneNumber = phoneNumber;
            this.context.SaveChanges();
        }
    }
}
