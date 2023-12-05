using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using Horizontal.Mapping;
using Horizontal.Models;
using Horizontal.Models.Navigation;
using Horizontal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Horizontal.Controllers
{
    public class HomeController : Controller
    {
        private INavigationService _navigationService;
        private IArticleRepository _articleRepository;
        private ITagRepository _tagRepository;

        public HomeController(INavigationService navigationService,
                              IArticleRepository articleRepository,
                              ITagRepository tagRepository)
        {
            _navigationService = navigationService;
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Main()
        {
            var mainModel = new MainModel(_navigationService, _tagRepository);

            foreach (var article in _articleRepository.Articles.Where(x => x.IsPublished).OrderByDescending(x => x.Created))
                mainModel.Articles.Add(HorizontalMapper.MapArticleModel(article, _navigationService, _tagRepository));

            return View(mainModel);
        }

        public IActionResult SignupForNewsletter(string email)
        {
            return View();
        }
    }
}
