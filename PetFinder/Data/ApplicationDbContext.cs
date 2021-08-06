using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetFinder.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<SearchPost> SearchPosts { get; init; }

        public DbSet<Pet> Pets { get; init; }

        public DbSet<Specie> Species { get; init; }

        public DbSet<City> Cities { get; init; }

        public DbSet<Comment> Comments { get; init; }

        public DbSet<SearchPostType> SearchPostTypes { get; init; }

        public DbSet<Size> Sizes { get; init; }

        public DbSet<ResourcePost> ResourcePosts { get; init; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<City>()
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Entity<Specie>()
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Entity<Owner>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Owner>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<City>()
                .HasMany(x => x.SearchPosts)
                .WithOne(x => x.City)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Specie>()
                .HasMany(x => x.Pets)
                .WithOne(x => x.Species)
                .HasForeignKey(x => x.SpeciesId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<SearchPostType>()
                .HasMany(x => x.SearchPosts)
                .WithOne(x => x.SearchPostType)
                .HasForeignKey(x => x.SearchPostTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
