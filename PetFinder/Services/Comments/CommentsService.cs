using PetFinder.Data;
using PetFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext context;

        public CommentsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddResourcePostComment(string comment, string resourcePostId, string userId)
        {
            var newComment = new Comment
            {
                Content = comment,
                CreatedOn = DateTime.UtcNow,
                ResourcePostId = resourcePostId,
                UserId = userId,
            };

            this.context.Comments.Add(newComment);
            this.context.SaveChanges();
        }

        public void AddSearchPostComment(string comment, string searchPostId, string userId)
        {
            var newComment = new Comment
            {
                Content = comment,
                CreatedOn = DateTime.UtcNow,
                SearchPostId = searchPostId,
                UserId = userId,
            };

            this.context.Comments.Add(newComment);
            this.context.SaveChanges();
        }

        public Tuple<bool, string> Delete(string id, string type)
        {
            var comment = this.context
                .Comments
                .FirstOrDefault(comment => comment.Id == id);

            if(comment == null)
            {
                return Tuple.Create(false, "");
            }

            this.context.Comments.Remove(comment);

            this.context.SaveChanges();

            var redirectionString = type == "SearchPosts" ? comment.SearchPostId : comment.ResourcePostId;

            return Tuple.Create(true, redirectionString);
        }

        public bool UserOwnsComment(string commentId, string userId)
        {
            return this.context
                .Comments
                .Any(comment => comment.Id == commentId 
                    && comment.UserId == userId);
        }

    }
}
