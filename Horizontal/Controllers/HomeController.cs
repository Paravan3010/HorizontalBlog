using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using Horizontal.Domain.Repositories.EF;
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
        private ICategoryRepository _categoryRepository;
        private IGeneralSettingsRepository _generalSettingsRepository;
        private IArticleTagRepository _articleTagRepository;

        public HomeController(INavigationService navigationService,
                              IArticleRepository articleRepository,
                              ITagRepository tagRepository,
                              ICategoryRepository categoryRepository,
                              IGeneralSettingsRepository generalSettingsRepository,
                              IArticleTagRepository articleTagRepository)
        {
            _navigationService = navigationService;
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _generalSettingsRepository = generalSettingsRepository;
            _articleTagRepository = articleTagRepository;
        }

        public IActionResult Main(int page = 1)
        {
            var mainModel = new MainModel(_navigationService, _tagRepository, _categoryRepository, _generalSettingsRepository);

            var publishedArticles = _articleRepository.Articles.Where(x => x.IsPublished);
            foreach (var article in publishedArticles.Where(x => x.IsInFeed)
                                                     .OrderByDescending(x => x.Created)
                                                     .Skip((_generalSettingsRepository.GeneralSettings.FirstOrDefault()?.PageSize ?? 10) * (page - 1))
                                                     .Take(_generalSettingsRepository.GeneralSettings.FirstOrDefault()?.PageSize ?? 10))
            {
                mainModel.Articles.Add(HorizontalMapper.MapArticleModel(article, _navigationService, _tagRepository, 
                                                                        _categoryRepository, _articleTagRepository, _generalSettingsRepository));
            }

            var settings = _generalSettingsRepository.GeneralSettings.FirstOrDefault();

            mainModel.Page = page;
            mainModel.TotalNumberOfPages = (int)Math.Ceiling(publishedArticles.Count() / (double)(settings?.PageSize ?? 10));
            mainModel.ActionName = nameof(Main);
            mainModel.PageTitle = settings?.MainPageTitle ?? String.Empty;
            mainModel.PageDescription = settings?.MainPageDescription ?? String.Empty;

            return View(mainModel);
        }

        public IActionResult SignupForNewsletter(string email)
        {
            return View();
        }
    }
}
