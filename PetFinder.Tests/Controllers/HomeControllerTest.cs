using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PetFinder.Controllers;
using PetFinder.Data.Models;
using PetFinder.Services.SearchPosts.Models;
using FluentAssertions;

namespace PetFinder.Tests.Controllers
{
    public class HomeControllerTest
    {

        [Fact]
        public void IndexShouldReturnCorrectView()
        {
            MyController<HomeController>
                .Instance()
                .WithData(GetSearchPosts())
                .Calling(c => c.Index())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<LatestSearchPostsServiceModel>>()
                    .Passing(m => m.Should().HaveCount(3)));
        }

        [Fact]
        public void ErrorShouldReturnView()
        {
            MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
        }

        private static IEnumerable<SearchPost> GetSearchPosts()
        {
            return Enumerable.Range(0, 10).Select(i => new SearchPost());
        }
    }
}
