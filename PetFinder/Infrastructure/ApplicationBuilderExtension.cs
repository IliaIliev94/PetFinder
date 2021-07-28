using Microsoft.AspNetCore.Builder;
using PetFinder.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PetFinder.Data.Models;

namespace PetFinder.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {

            var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<ApplicationDbContext>();

            data.Database.Migrate();

            SeedCities(data);

            SeedSizes(data);

            SeedSpecies(data);

            SeedSearchPostTypes(data);

            return app;
        }

        private static void SeedCities(ApplicationDbContext data)
        {
            if (data.Cities.Any())
            {
                return;
            }

            data.Cities.AddRange(new[]
            {
                new City {Name = "Sofia"},
                new City {Name = "Varna"},
                new City {Name = "Plovdiv"},
                new City {Name = "Burgas"},
                new City {Name = "Veliko Turnovo"},
                new City {Name = "Blagoevgrad"},
                new City {Name = "Vidin"},
                new City {Name = "Vratza"},
                new City {Name = "Gabrovo"},
                new City {Name = "Dobrich"},
                new City {Name = "Kurdzhali"},
                new City {Name = "Kustendil"},
                new City {Name = "Lovech"},
                new City {Name = "Montana"},
                new City {Name = "Pazardjik"},
                new City {Name = "Pernik"},
                new City {Name = "Pleven"},
                new City {Name = "Razgrad"},
                new City {Name = "Ruse"},
                new City {Name = "Silistra"},
                new City {Name = "Sliven"},
                new City {Name = "Smolqn"},
                new City {Name = "Stara Zagora"},
                new City {Name = "Turgovishte"},
                new City {Name = "Haskovo"},
                new City {Name = "Shumen"},
                new City {Name = "Yambol"},
            });

            data.SaveChanges();
        }

        private static void SeedSizes(ApplicationDbContext data)
        {
            if(data.Sizes.Any())
            {
                return;
            }

            data.Sizes.AddRange(new[]
            {
                new Size {Type = "Very small"},
                new Size {Type = "Small"},
                new Size {Type = "Medium"},
                new Size {Type = "Large"},
                new Size {Type = "Very large"},
            });

            data.SaveChanges();
        }

        private static void SeedSpecies(ApplicationDbContext data)
        {
            if (data.Species.Any())
            {
                return;
            }

            data.Species.AddRange(new[]
            {
                new Specie {Name = "Dog"},
                new Specie {Name = "Cat"},
                new Specie {Name = "Lizard"},
                new Specie {Name = "Rabbit"},
                new Specie {Name = "Snake"},
                new Specie {Name = "Horse"},
                new Specie {Name = "Bird"},
                new Specie {Name = "Feret"},
                new Specie {Name = "Tortoise"},
                new Specie {Name = "Goat"},
                new Specie {Name = "Sheep"},
                new Specie {Name = "Pig"},
                new Specie {Name = "Other"},
            });

            data.SaveChanges();
        }

        private static void SeedSearchPostTypes(ApplicationDbContext data)
        {
            if(data.SearchPostTypes.Any())
            {
                return;
            }

            data.SearchPostTypes.AddRange(new[]
            {
                new SearchPostType {Name = "Lost"},
                new SearchPostType {Name = "Found"}
            });

            data.SaveChanges();
        }
    }
}
