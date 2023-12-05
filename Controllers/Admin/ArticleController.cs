using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using Horizontal.Mapping;
using Horizontal.Migrations;
using Horizontal.Models.Admin;
using Horizontal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Horizontal.Controllers.Admin
{
    [Authorize]
    [Route("admin/article")]
    public class ArticleController : Controller
    {
        private ITagRepository _tagRepository;
        private IArticleRepository _articleRepository;
        private ICategoryRepository _categoryRepository;
        private ICustomUrlRepository _customUrlRepository;
        private ICustomUrlProviderService _customUrlProviderService;


        public ArticleController(ITagRepository tagRepository,
                                 IArticleRepository articleRepository,
                                 ICategoryRepository categoryRepository,
                                 ICustomUrlRepository customUrlRepository,
                                 ICustomUrlProviderService customUrlProviderService)
        {
            _tagRepository = tagRepository;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _customUrlRepository = customUrlRepository;
            _customUrlProviderService = customUrlProviderService;
        }

        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var model = new List<ArticlePreviewModel>();
            foreach (var article in _articleRepository.Articles.OrderByDescending(x => x.LastUpdated))
                model.Add(HorizontalMapper.MapArticlePreviewModel(article, _customUrlProviderService));

            return View("Views/Admin/Article/Index.cshtml", model);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            var model = new AdminArticleModel();
            foreach (var article in _articleRepository.Articles)
            {
                var dropdownModel = new ArticleDropdownModel()
                {
                    Id = article.Id,
                    Title = String.IsNullOrEmpty(article.LongTitle) ? article.ShortTitle : article.LongTitle
                };
                model.AllArticles.Add(dropdownModel);
            }
            model.AllArticles = model.AllArticles.OrderBy(x => x.Title).ToList();

            return View("Views/Admin/Article/Create.cshtml", model);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AdminArticleModel model)
        {
            // Check ModelState

            var article = HorizontalMapper.MapArticle(model, _categoryRepository, _tagRepository, _articleRepository);
            var articleId = _articleRepository.CreateArticle(article);

            if (!String.IsNullOrEmpty(model.CustomUrl))
                _customUrlRepository.CreateMapping(new CustomUrl { NewUrl = model.CustomUrl, OriginalUrl = $"/Article/FullArticle?articleId={articleId}" });

            return Redirect("/admin/article");
        }

        [HttpGet]
        [Route("detail")]
        public IActionResult Detail(int articleId)
        {
            var article = _articleRepository.Articles.Where(x => x.Id == articleId).FirstOrDefault();
            if (article == null)
                return NotFound();

            return View("Views/Admin/Article/Detail.cshtml", HorizontalMapper.MapAdminArticleModel(article, _customUrlRepository, _articleRepository));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AdminArticleModel model)
        {
            var article = _articleRepository.Articles.Where(x => x.Id == model.Id).FirstOrDefault();
            if (article == null)
                return NotFound();

            article = HorizontalMapper.MapArticle(model, _categoryRepository, _tagRepository, _articleRepository, article);
            _articleRepository.SaveArticle(article);

            var customUrlMapping = _customUrlRepository.CustomUrls.Where(x => x.OriginalUrl == $"/Article/FullArticle?articleId={article.Id}").FirstOrDefault();
            if (customUrlMapping != null)
            {
                if (String.IsNullOrEmpty(model.CustomUrl))
                {
                    _customUrlRepository.DeleteMapping(customUrlMapping);
                }
                else
                {
                    customUrlMapping.NewUrl = model.CustomUrl;
                    _customUrlRepository.SaveMapping(customUrlMapping);
                }
            }
            else if (!String.IsNullOrEmpty(model.CustomUrl))
            {
                customUrlMapping = new CustomUrl()
                {
                    OriginalUrl = $"/Article/FullArticle?articleId={article.Id}",
                    NewUrl = model.CustomUrl
                };
                _customUrlRepository.CreateMapping(customUrlMapping);
            }

            return Redirect($"/admin/article/detail?articleId={article.Id}");
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete(int articleId)
        {
            var article = _articleRepository.Articles.Where(x => x.Id == articleId).FirstOrDefault();
            if (article == null)
                return NotFound();

            // Delete CustomUrl mapping if exists
            var customUrlMapping = _customUrlRepository.CustomUrls.Where(x => x.OriginalUrl == $"/Article/FullArticle?articleId={article.Id}").FirstOrDefault();
            if (customUrlMapping != null)
                _customUrlRepository.DeleteMapping(customUrlMapping);

            if (article.IsPublished)
            {
                article.IsPublished = false;
                _articleRepository.SaveArticle(article);
            }
            else
            {
                _articleRepository.DeleteArticle(article);
            }

            return Redirect("/admin/article");
        }
    }
}
