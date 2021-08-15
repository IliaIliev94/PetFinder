using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Comments.Models
{
    public class CommentServiceModel
    {
        public string Id { get; init; }

        public string Content { get; init; }

        public DateTime CreatedOn { get; init; }

        public string Username { get; init; }
    }
}
