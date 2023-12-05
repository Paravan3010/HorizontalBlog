using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using Horizontal.Mapping;
using Horizontal.Migrations;
using Horizontal.Models.Admin;
using Horizontal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Controllers.Admin
{
    [Authorize]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private IArticleRepository _articleRepository;
        private ICategoryRepository _categoryRepository;
        private ICustomUrlRepository _customUrlRepository;
        private ICustomUrlProviderService _customUrlProviderService;


        public CategoryController(IArticleRepository articleRepository,
                                  ICategoryRepository categoryRepository,
                                  ICustomUrlRepository customUrlRepository,
                                  ICustomUrlProviderService customUrlProviderService)
        {
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
            var model = new List<CategoryPreviewModel>();
            foreach (var category in _categoryRepository.Categories.OrderByDescending(x => x.Id))
                model.Add(HorizontalMapper.MapCategoryPreviewModel(category, _customUrlProviderService));

            return View("Views/Admin/Category/Index.cshtml", model);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View("Views/Admin/Category/Create.cshtml");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AdminCategoryModel model)
        {
            // Check ModelState

            var category = HorizontalMapper.MapCategory(model, _categoryRepository, _articleRepository);
            var categoryId = _categoryRepository.CreateCategory(category);

            if (!String.IsNullOrEmpty(model.CustomUrl))
                _customUrlRepository.CreateMapping(new CustomUrl { NewUrl = model.CustomUrl, OriginalUrl = $"/Category/Category?categoryId={categoryId}" });

            return Redirect("/admin/category");
        }

        [HttpGet]
        [Route("detail")]
        public IActionResult Detail(int categoryId)
        {
            var category = _categoryRepository.Categories.Where(x => x.Id == categoryId).FirstOrDefault();
            if (category == null)
                return NotFound();

            return View("Views/Admin/Category/Detail.cshtml", HorizontalMapper.MapAdminCategoryModel(category, _customUrlRepository));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AdminCategoryModel model)
        {
            var category = _categoryRepository.Categories.Where(x => x.Id == model.Id).FirstOrDefault();
            if (category == null)
                return NotFound();

            category = HorizontalMapper.MapCategory(model, _categoryRepository, _articleRepository, category);
            _categoryRepository.SaveCategory(category);


            var customUrlMapping = _customUrlRepository.CustomUrls.Where(x => x.OriginalUrl == $"/Category/Category?categoryId={category.Id}").FirstOrDefault();
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
                    OriginalUrl = $"/Category/Category?categoryId={category.Id}",
                    NewUrl = model.CustomUrl
                };
                _customUrlRepository.CreateMapping(customUrlMapping);
            }

            return Redirect($"/admin/category/detail?categoryId={category.Id}");
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete(int categoryId)
        {
            var category = _categoryRepository.Categories.Where(x => x.Id == categoryId).FirstOrDefault();
            if (category == null)
                return NotFound();

            // Delete CustomUrl mapping if exists
            var customUrlMapping = _customUrlRepository.CustomUrls.Where(x => x.OriginalUrl == $"/Category/Category?categoryId={category.Id}").FirstOrDefault();
            if (customUrlMapping != null)
                _customUrlRepository.DeleteMapping(customUrlMapping);

            if (category.IsPublished)
            {
                category.IsPublished = false;
                _categoryRepository.SaveCategory(category);
            }
            else
            {
                _categoryRepository.DeleteCategory(category);
            }

            return Redirect("/admin/category");
        }
    }
}
