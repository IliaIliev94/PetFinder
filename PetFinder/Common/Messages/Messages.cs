using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinder.Common.Messages
{
    public static class Messages
    {
        public const string SuccessAddMessage = "Successfully added {0}.";
        public const string SuccessEditMessage = "Successfully edited {0}.";
        public const string SuccessDeleteMessage = "Successfully deleted {0}.";
        public const string SuccessSaveMessage = "Successfully saved {0}.";
        public const string SuccessRemoveSavedMessage = "Successfully removed {0} from saved {0}s.";

        public const string ErrorAddMessage = "Adding new {0} failed. Please try again!";
        public const string ErrorEditMessage = "Editing {0} failed. Please try again!";
        public const string ErrorDeleteMessage = "Deleting {0} failed. Please try again!";

        public const string UnauthorizedMessage = "Only {0}s are authorized to do that action. You must become a {0} first.";
    }
}
