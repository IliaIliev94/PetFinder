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

            base.OnModelCreating(builder);
        }
    }
}
