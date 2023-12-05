using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
