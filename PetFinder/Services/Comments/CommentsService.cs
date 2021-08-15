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

        public void AddResourcePostComment(string comment, string id)
        {
            var newComment = new Comment
            {
                Content = comment,
                CreatedOn = DateTime.UtcNow,
                ResourcePostId = id,
            };

            this.context.Comments.Add(newComment);
            this.context.SaveChanges();
        }

        public void AddSearchPostComment(string comment, string id)
        {
            throw new NotImplementedException();
        }
    }
}
