using Microsoft.AspNetCore.Builder;
using PetFinder.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PetFinder.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;

using static PetFinder.WebConstants;

namespace PetFinder.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {

            var scopedServices = app.ApplicationServices.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;

            var data = serviceProvider.GetRequiredService<ApplicationDbContext>();

            data.Database.Migrate();

            SeedCities(data);

            SeedSizes(data);

            SeedSpecies(data);

            SeedSearchPostTypes(data);

            SeedResourcePosts(data);

            SeedAdministrator(serviceProvider);

            return app;
        }

        private static void SeedResourcePosts(ApplicationDbContext data)
        {
            if(data.ResourcePosts.Any())
            {
                return;
            }

            data.ResourcePosts.AddRange(new[]
            {

                new ResourcePost
                {
                    Title = "Post1",
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://www.thesprucepets.com/thmb/d-OLDGZ8-cDohlNuKdOC1CIkmng=/1000x1000/smart/filters:no_upscale()/GettyImages-962608834-fd496cfed51e4d2abe61c0af864fa681.jpg",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus fermentum dui velit, ut fermentum magna condimentum sed. Nulla accumsan orci diam, eu commodo tellus congue vitae. Etiam non ullamcorper mi. Maecenas quis vulputate magna. Sed finibus laoreet fringilla. Nam finibus tincidunt felis, nec condimentum neque viverra vitae. Nunc molestie elit nec gravida euismod. Pellentesque quis commodo nunc. Quisque efficitur vehicula enim, vel eleifend metus sollicitudin at. Nunc at odio leo. Integer nec orci id massa euismod pretium.

                                Praesent imperdiet mi quis vestibulum pellentesque. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus aliquam est eget diam gravida, quis consequat ligula ullamcorper. Quisque vestibulum mollis leo, id sodales enim dictum sit amet. Sed dapibus velit id nisi tristique efficitur nec vel ante. Duis ultricies mattis molestie. Praesent at velit tincidunt, pharetra elit non, auctor velit. Vivamus vel gravida lectus. Integer ultricies velit et ultricies tincidunt. Aenean scelerisque ut lectus a ullamcorper. Vivamus dapibus imperdiet arcu eu lobortis.

                                Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Integer tempus augue eros, non cursus nisl blandit a. Nunc ullamcorper quam at dolor auctor, sit amet porta sem porta. Suspendisse vel molestie augue. Aenean quis est sit amet magna hendrerit tristique. Suspendisse sed enim justo. Morbi sagittis tempor nunc sed suscipit.

                                Pellentesque et accumsan magna, eu tempus ligula. Donec varius ante urna, non scelerisque dolor convallis vitae. Duis commodo egestas lorem, et egestas nibh laoreet vel. Nullam auctor luctus vulputate. Aliquam erat volutpat. Curabitur libero turpis, aliquet volutpat facilisis et, ornare eu massa. Fusce et vehicula sem, sed bibendum libero. Sed non interdum dolor. Praesent lectus eros, convallis non convallis eu, laoreet volutpat velit. Fusce aliquam, magna laoreet molestie gravida, lectus quam feugiat mauris, eleifend mattis justo ex quis eros. Aliquam ultricies metus a velit accumsan posuere. Aenean pulvinar ligula. "
                },
                 new ResourcePost
                {
                    Title = "Post2",
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://www.thesprucepets.com/thmb/d-OLDGZ8-cDohlNuKdOC1CIkmng=/1000x1000/smart/filters:no_upscale()/GettyImages-962608834-fd496cfed51e4d2abe61c0af864fa681.jpg",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus fermentum dui velit, ut fermentum magna condimentum sed. Nulla accumsan orci diam, eu commodo tellus congue vitae. Etiam non ullamcorper mi. Maecenas quis vulputate magna. Sed finibus laoreet fringilla. Nam finibus tincidunt felis, nec condimentum neque viverra vitae. Nunc molestie elit nec gravida euismod. Pellentesque quis commodo nunc. Quisque efficitur vehicula enim, vel eleifend metus sollicitudin at. Nunc at odio leo. Integer nec orci id massa euismod pretium.

                                Praesent imperdiet mi quis vestibulum pellentesque. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus aliquam est eget diam gravida, quis consequat ligula ullamcorper. Quisque vestibulum mollis leo, id sodales enim dictum sit amet. Sed dapibus velit id nisi tristique efficitur nec vel ante. Duis ultricies mattis molestie. Praesent at velit tincidunt, pharetra elit non, auctor velit. Vivamus vel gravida lectus. Integer ultricies velit et ultricies tincidunt. Aenean scelerisque ut lectus a ullamcorper. Vivamus dapibus imperdiet arcu eu lobortis.

                                Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Integer tempus augue eros, non cursus nisl blandit a. Nunc ullamcorper quam at dolor auctor, sit amet porta sem porta. Suspendisse vel molestie augue. Aenean quis est sit amet magna hendrerit tristique. Suspendisse sed enim justo. Morbi sagittis tempor nunc sed suscipit.

                                Pellentesque et accumsan magna, eu tempus ligula. Donec varius ante urna, non scelerisque dolor convallis vitae. Duis commodo egestas lorem, et egestas nibh laoreet vel. Nullam auctor luctus vulputate. Aliquam erat volutpat. Curabitur libero turpis, aliquet volutpat facilisis et, ornare eu massa. Fusce et vehicula sem, sed bibendum libero. Sed non interdum dolor. Praesent lectus eros, convallis non convallis eu, laoreet volutpat velit. Fusce aliquam, magna laoreet molestie gravida, lectus quam feugiat mauris, eleifend mattis justo ex quis eros. Aliquam ultricies metus a velit accumsan posuere. Aenean pulvinar ligula. "
                },
                  new ResourcePost
                {
                    Title = "Post3",
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://www.thesprucepets.com/thmb/d-OLDGZ8-cDohlNuKdOC1CIkmng=/1000x1000/smart/filters:no_upscale()/GettyImages-962608834-fd496cfed51e4d2abe61c0af864fa681.jpg",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus fermentum dui velit, ut fermentum magna condimentum sed. Nulla accumsan orci diam, eu commodo tellus congue vitae. Etiam non ullamcorper mi. Maecenas quis vulputate magna. Sed finibus laoreet fringilla. Nam finibus tincidunt felis, nec condimentum neque viverra vitae. Nunc molestie elit nec gravida euismod. Pellentesque quis commodo nunc. Quisque efficitur vehicula enim, vel eleifend metus sollicitudin at. Nunc at odio leo. Integer nec orci id massa euismod pretium.

                                Praesent imperdiet mi quis vestibulum pellentesque. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus aliquam est eget diam gravida, quis consequat ligula ullamcorper. Quisque vestibulum mollis leo, id sodales enim dictum sit amet. Sed dapibus velit id nisi tristique efficitur nec vel ante. Duis ultricies mattis molestie. Praesent at velit tincidunt, pharetra elit non, auctor velit. Vivamus vel gravida lectus. Integer ultricies velit et ultricies tincidunt. Aenean scelerisque ut lectus a ullamcorper. Vivamus dapibus imperdiet arcu eu lobortis.

                                Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Integer tempus augue eros, non cursus nisl blandit a. Nunc ullamcorper quam at dolor auctor, sit amet porta sem porta. Suspendisse vel molestie augue. Aenean quis est sit amet magna hendrerit tristique. Suspendisse sed enim justo. Morbi sagittis tempor nunc sed suscipit.

                                Pellentesque et accumsan magna, eu tempus ligula. Donec varius ante urna, non scelerisque dolor convallis vitae. Duis commodo egestas lorem, et egestas nibh laoreet vel. Nullam auctor luctus vulputate. Aliquam erat volutpat. Curabitur libero turpis, aliquet volutpat facilisis et, ornare eu massa. Fusce et vehicula sem, sed bibendum libero. Sed non interdum dolor. Praesent lectus eros, convallis non convallis eu, laoreet volutpat velit. Fusce aliquam, magna laoreet molestie gravida, lectus quam feugiat mauris, eleifend mattis justo ex quis eros. Aliquam ultricies metus a velit accumsan posuere. Aenean pulvinar ligula. "
                },
                   new ResourcePost
                {
                    Title = "Post4",
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://www.thesprucepets.com/thmb/d-OLDGZ8-cDohlNuKdOC1CIkmng=/1000x1000/smart/filters:no_upscale()/GettyImages-962608834-fd496cfed51e4d2abe61c0af864fa681.jpg",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus fermentum dui velit, ut fermentum magna condimentum sed. Nulla accumsan orci diam, eu commodo tellus congue vitae. Etiam non ullamcorper mi. Maecenas quis vulputate magna. Sed finibus laoreet fringilla. Nam finibus tincidunt felis, nec condimentum neque viverra vitae. Nunc molestie elit nec gravida euismod. Pellentesque quis commodo nunc. Quisque efficitur vehicula enim, vel eleifend metus sollicitudin at. Nunc at odio leo. Integer nec orci id massa euismod pretium.

                                Praesent imperdiet mi quis vestibulum pellentesque. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus aliquam est eget diam gravida, quis consequat ligula ullamcorper. Quisque vestibulum mollis leo, id sodales enim dictum sit amet. Sed dapibus velit id nisi tristique efficitur nec vel ante. Duis ultricies mattis molestie. Praesent at velit tincidunt, pharetra elit non, auctor velit. Vivamus vel gravida lectus. Integer ultricies velit et ultricies tincidunt. Aenean scelerisque ut lectus a ullamcorper. Vivamus dapibus imperdiet arcu eu lobortis.

                                Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Integer tempus augue eros, non cursus nisl blandit a. Nunc ullamcorper quam at dolor auctor, sit amet porta sem porta. Suspendisse vel molestie augue. Aenean quis est sit amet magna hendrerit tristique. Suspendisse sed enim justo. Morbi sagittis tempor nunc sed suscipit.

                                Pellentesque et accumsan magna, eu tempus ligula. Donec varius ante urna, non scelerisque dolor convallis vitae. Duis commodo egestas lorem, et egestas nibh laoreet vel. Nullam auctor luctus vulputate. Aliquam erat volutpat. Curabitur libero turpis, aliquet volutpat facilisis et, ornare eu massa. Fusce et vehicula sem, sed bibendum libero. Sed non interdum dolor. Praesent lectus eros, convallis non convallis eu, laoreet volutpat velit. Fusce aliquam, magna laoreet molestie gravida, lectus quam feugiat mauris, eleifend mattis justo ex quis eros. Aliquam ultricies metus a velit accumsan posuere. Aenean pulvinar ligula. "
                }
            });

            data.SaveChanges();
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

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () => 
                { 
                    if(await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                        var role = new IdentityRole { Name = AdministratorRoleName };

                        await roleManager.CreateAsync(role);

                        const string adminEmail = "admin@pf.com";
                        const string adminPassword = "admin12";

                        var user = new IdentityUser
                        {
                            Email = adminEmail,
                            UserName = "Admin",
                        };

                        await userManager.CreateAsync(user, adminPassword);

                        await userManager.AddToRoleAsync(user, role.Name);

                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
