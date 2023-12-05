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

        public CategoryController(ICategoryRepository categoryRepository,
                                  INavigationService navigationService,
                                  IArticleRepository articleRepository,
                                  ITagRepository tagRepository)
        {
            _categoryRepository = categoryRepository;
            _navigationService = navigationService;
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Category(int categoryId)
        {
            var category = _categoryRepository.Categories.Where(x => x.Id == categoryId && x.IsPublished).FirstOrDefault();
            if (category == null)
                return NotFound();

            var model = HorizontalMapper.MapCategoryModel(category, _navigationService, _tagRepository);
            foreach (var article in _articleRepository.Articles.Where(x => x.Category != null && x.Category.Id == categoryId && x.IsPublished).OrderByDescending(x => x.Created))
                model.Articles.Add(HorizontalMapper.MapArticleModel(article, _navigationService, _tagRepository));

            return View(model);
        }

        public IActionResult Tag(string tagName)
        {
            var tag = _tagRepository.Tags.Where(x => x.IsPublished && x.Name == tagName).FirstOrDefault();
            if (tag == null)
                return NotFound();

            var model = HorizontalMapper.MapCategoryModel(tag, _navigationService, _tagRepository);
            foreach (var article in tag.Articles.Where(x => x.IsPublished).OrderByDescending(x => x.Created))
                model.Articles.Add(HorizontalMapper.MapArticleModel(article, _navigationService, _tagRepository));

            return View("Views/Category/Category.cshtml", model);
        }
    }
}
