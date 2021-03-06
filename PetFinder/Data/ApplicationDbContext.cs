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

        public DbSet<Owner> Owners { get; init; }

        public DbSet<UserSearchPost> SavedSearchPosts { get; init; }

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

            builder.Entity<SearchPost>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.Entity<Comment>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.Entity<UserSearchPost>()
                .HasKey(x => new { x.UserId, x.SearchPostId });

            builder.Entity<UserSearchPost>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.Entity<UserSearchPost>()
                .HasOne(x => x.SearchPost)
                .WithMany(x => x.SavedSearchPosts)
                .HasForeignKey(x => x.SearchPostId);

            base.OnModelCreating(builder);
        }
    }
}
