using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Comments
{
    public interface ICommentsService
    {
        bool AddSearchPostComment(string comment, string searchPostId, string userId);

        bool AddResourcePostComment(string comment, string resourcePostId, string userId);

        bool UserOwnsComment(string commentId, string userId);

        Tuple<bool, string> Delete(string id, string type);
    }
}
