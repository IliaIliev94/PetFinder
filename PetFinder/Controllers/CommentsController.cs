using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Services.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [Authorize]
        public IActionResult Add(string comment, string searchPostId, string resourcePostId)
        {
            if(resourcePostId != null)
            {
                this.commentsService.AddResourcePostComment(comment, resourcePostId);
                return this.RedirectToAction("Details", "Resources", new {Id = resourcePostId });
            }

            this.commentsService.AddSearchPostComment(comment, searchPostId);
            return this.RedirectToAction("Details", "SearchPosts", new { Id = searchPostId });
        }
    }
}
