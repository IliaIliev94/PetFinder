using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using PetFinder.Data.Models;
using PetFinder.Infrastructure;
using PetFinder.Services.Comments;
using PetFinder.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetFinder.Tests.Services
{
    public class CommentServiceTest
    {
        private ICommentsService commentService;
        [Theory]
        [InlineData("Test", "TestId")]
        public void AddShouldAddComments(string comment, string userId)
        {
            var database = DatabaseMock.Instance;
            this.commentService = new CommentsService(database);

            database.ResourcePosts.Add(new ResourcePost { Id = "1" });
            database.Users.Add(new IdentityUser { Id = userId });
            database.SaveChanges();

            database.Comments.Should().HaveCount(0);
            var isCreationSuccessfull = this.commentService.AddResourcePostComment(comment, "1", userId);
            isCreationSuccessfull.Should().BeTrue();
            database.Comments.Should().HaveCount(1);
        }

        [Fact]
        public void AddShouldReturnFalseIfPostDoesNotExist()
        {
            var database = DatabaseMock.Instance;
            this.commentService = new CommentsService(database);

            database.ResourcePosts.Add(new ResourcePost { Id = "1" });
            database.Users.Add(new IdentityUser { Id = "TestId" });
            database.SaveChanges();

            var isCreationSuccessfull = this.commentService.AddResourcePostComment("Comment", "User", "Test");
            isCreationSuccessfull.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test", "TestId")]
        public void DeleteShouldWorkCorrectly(string comment, string userId)
        {
            var database = DatabaseMock.Instance;
            this.commentService = new CommentsService(database);

            database.ResourcePosts.Add(new ResourcePost { Id = "1" });
            database.Users.Add(new IdentityUser { Id = userId });
            database.SaveChanges();

            database.Comments.Should().HaveCount(0);
            this.commentService.AddResourcePostComment(comment, "1", userId);
            database.Comments.Should().HaveCount(1);
            var commentId = database.Comments.FirstOrDefault(c => c.Content == comment).Id;
            var isDeleteSuccessfull = this.commentService.Delete(commentId, null);
            isDeleteSuccessfull.Item1.Should().BeTrue();
            database.Comments.Should().HaveCount(0);
        }
    }
}
