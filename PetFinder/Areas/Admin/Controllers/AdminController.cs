
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static PetFinder.WebConstants;

namespace PetFinder.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public abstract class AdminController : Controller
    {
    }
}
