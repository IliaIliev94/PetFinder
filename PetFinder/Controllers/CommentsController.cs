using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Infrastructure;
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

            if(String.IsNullOrWhiteSpace(comment))
            {
                return this.BadRequest();
            }

            if(resourcePostId != null)
            {
                this.commentsService.AddResourcePostComment(comment, resourcePostId, this.User.GetId());
                return this.RedirectToAction("Details", "Resources", new {Id = resourcePostId });
            }

            this.commentsService.AddSearchPostComment(comment, searchPostId, this.User.GetId());
            return this.RedirectToAction("Details", "SearchPosts", new { Id = searchPostId });
        }

        [Authorize]
        public IActionResult Delete(string id, string type)
        {
            if(!this.User.IsAdmin() && !this.commentsService.UserOwnsComment(id, this.User.GetId()))
            {
                return this.Unauthorized();
            }

            var isDeleteSuccessfull = this.commentsService.Delete(id, type);

            if(!isDeleteSuccessfull.Item1)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("Details", type, new { Id = isDeleteSuccessfull.Item2 });
        }
    }
}
