using Horizontal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Controllers.Admin
{
    [Authorize]
    [Route("admin/general")]
    public class GeneralSettingsController : Controller
    {
        private readonly INavigationService _navigationService;

        public GeneralSettingsController( INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View("Views/Admin/GeneralSettings/Index.cshtml");
        }

        [HttpGet]
        [Route("ActualizeLeftNavBar")]
        public IActionResult ActualizeLeftNavBar()
        {
            _navigationService.ActualizeCategoryNavigation();

            return RedirectToAction("Index");
        }
    }
}
