using Horizontal.Domain.Repositories;
using Horizontal.Mapping;
using Horizontal.Models;
using Horizontal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleRepository _articleRepository;
        private INavigationService _navigationService;
        private ITagRepository _tagRepository;
        private ICategoryRepository _categoryRepository;
        private IArticleTagRepository _articleTagRepository;

        public ArticleController(IArticleRepository articleRepository,
                                 INavigationService navigationService,
                                 ITagRepository tagRepository,
                                 ICategoryRepository categoryRepository,
                                 IArticleTagRepository articleTagRepository)
        {
            _articleRepository = articleRepository;
            _navigationService = navigationService;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _articleTagRepository = articleTagRepository;
        }

        public IActionResult FullArticle(int articleId)
        {
            var article = _articleRepository.Articles.Where(x => x.Id == articleId).FirstOrDefault();
            if (article == null)
                return NotFound();

            if (!article.IsPublished && !User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("FullArticle", new { articleId }) });

            _articleRepository.IncreaseVisitCount(articleId);
            return View(HorizontalMapper.MapArticleModel(article, _navigationService, _tagRepository, _categoryRepository, _articleTagRepository));
        }
    }
}
