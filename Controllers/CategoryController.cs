using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using Horizontal.Mapping;
using Horizontal.Models;
using Horizontal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Controllers
{
    public class CategoryController : Controller
    {
        private IArticleRepository _articleRepository;
        private ICategoryRepository _categoryRepository;
        private INavigationService _navigationService;
        private ITagRepository _tagRepository;
        private IGeneralSettingsRepository _generalSettingsRepository;

        public CategoryController(ICategoryRepository categoryRepository,
                                  INavigationService navigationService,
                                  IArticleRepository articleRepository,
                                  ITagRepository tagRepository,
                                  IGeneralSettingsRepository generalSettingsRepository)
        {
            _categoryRepository = categoryRepository;
            _navigationService = navigationService;
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _generalSettingsRepository = generalSettingsRepository;
        }

        public IActionResult Category(int categoryId, int page = 1)
        {
            var category = _categoryRepository.Categories.Where(x => x.Id == categoryId && x.IsPublished).FirstOrDefault();
            if (category == null)
                return NotFound();

            var model = HorizontalMapper.MapCategoryModel(category, _navigationService, _tagRepository, _categoryRepository);
            var articlesInCategory = _articleRepository.Articles.Where(x => x.Category != null && x.Category.Id == categoryId && x.IsPublished);
            foreach (var article in articlesInCategory.OrderByDescending(x => x.Created)
                                                      .Skip((_generalSettingsRepository.GeneralSettings.FirstOrDefault()?.PageSize ?? 10) * (page - 1))
                                                      .Take(_generalSettingsRepository.GeneralSettings.FirstOrDefault()?.PageSize ?? 10))
            {
                model.Articles.Add(HorizontalMapper.MapArticleModel(article, _navigationService, _tagRepository, _categoryRepository));
            }
            model.Page = page;            
            model.TotalNumberOfPages = (int)Math.Ceiling(articlesInCategory.Count() / (double)(_generalSettingsRepository.GeneralSettings.FirstOrDefault()?.PageSize ?? 10));

            return View(model);
        }

        public IActionResult Tag(string tagName, int page = 1)
        {
            var tag = _tagRepository.Tags.Where(x => x.IsPublished && x.Name == tagName).FirstOrDefault();
            if (tag == null)
                return NotFound();

            var model = HorizontalMapper.MapCategoryModel(tag, _navigationService, _tagRepository, _categoryRepository);
            var articlesWithTag = tag.Articles.Where(x => x.IsPublished);
            foreach (var article in articlesWithTag.OrderByDescending(x => x.Created)
                                                   .Skip((_generalSettingsRepository.GeneralSettings.FirstOrDefault()?.PageSize ?? 10) * (page - 1))
                                                   .Take(_generalSettingsRepository.GeneralSettings.FirstOrDefault()?.PageSize ?? 10))
            {
                model.Articles.Add(HorizontalMapper.MapArticleModel(article, _navigationService, _tagRepository, _categoryRepository));
            }
            model.Page = page;
            model.TotalNumberOfPages = (int)Math.Ceiling(articlesWithTag.Count() / (double)(_generalSettingsRepository.GeneralSettings.FirstOrDefault()?.PageSize ?? 10));

            return View("Views/Category/Category.cshtml", model);
        }
    }
}
