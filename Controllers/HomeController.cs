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

        public IActionResult Main(int page = 1)
        {
            var mainModel = new MainModel(_navigationService, _tagRepository);

            var publishedArticles = _articleRepository.Articles.Where(x => x.IsPublished);
            foreach (var article in publishedArticles.OrderByDescending(x => x.Created)
                                                     .Skip(Program.ARTICLES_PER_PAGE * (page - 1))
                                                     .Take(Program.ARTICLES_PER_PAGE))
            {
                mainModel.Articles.Add(HorizontalMapper.MapArticleModel(article, _navigationService, _tagRepository));
            }

            mainModel.Page = page;
            mainModel.TotalNumberOfPages = (int)Math.Ceiling(publishedArticles.Count() / (double)Program.ARTICLES_PER_PAGE);
            mainModel.ActionName = nameof(Main);

            return View(mainModel);
        }

        public IActionResult SignupForNewsletter(string email)
        {
            return View();
        }
    }
}
