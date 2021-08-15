using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Services.Comments
{
    public interface ICommentsService
    {
        void AddSearchPostComment(string comment, string id);

        void AddResourcePostComment(string comment, string id);
    }
}
