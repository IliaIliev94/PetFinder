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

            var isCreationSuccessfull = false;

            if(resourcePostId != null)
            {
                isCreationSuccessfull = this.commentsService.AddResourcePostComment(comment, resourcePostId, this.User.GetId());
            }
            else
            {
                isCreationSuccessfull = this.commentsService.AddSearchPostComment(comment, searchPostId, this.User.GetId());
            }
            
            if(!isCreationSuccessfull)
            {
                return this.BadRequest();
            }

            var controllerToRedirectTo = resourcePostId == null ? "SearchPosts" : "ResourcePosts";
            return this.RedirectToAction("Details", controllerToRedirectTo , new { Id = searchPostId });
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
