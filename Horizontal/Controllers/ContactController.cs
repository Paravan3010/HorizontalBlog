using Horizontal.Models;
using Horizontal.Domain.Repositories;
using Horizontal.Services;
using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Controllers
{
    public class ContactController : Controller
    {
        private readonly INavigationService _navigationService;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ContactController(INavigationService navigationService, ITagRepository tagRepository, ICategoryRepository categoryRepository)
        {
            _navigationService = navigationService;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var model = new ContactModel(_navigationService, _tagRepository, _categoryRepository);
            return View(model);
        }
    }
}
