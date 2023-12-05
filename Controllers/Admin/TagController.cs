using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using Horizontal.Mapping;
using Horizontal.Models.Admin;
using Horizontal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Controllers.Admin
{
    [Authorize]
    [Route("admin/tag")]
    public class TagController : Controller
    {
        private ITagRepository _tagRepository;
        private IArticleRepository _articleRepository;
        private ICustomUrlRepository _customUrlRepository;
        private ICustomUrlProviderService _customUrlProviderService;


        public TagController(ITagRepository tagRepository,
                             IArticleRepository articleRepository,
                             ICustomUrlRepository customUrlRepository,
                             ICustomUrlProviderService customUrlProviderService)
        {
            _tagRepository = tagRepository;
            _articleRepository = articleRepository;
            _customUrlRepository = customUrlRepository;
            _customUrlProviderService = customUrlProviderService;
        }

        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var model = new List<TagPreviewModel>();
            foreach (var tag in _tagRepository.Tags.OrderBy(x => x.Name))
                model.Add(HorizontalMapper.MapTagPreviewModel(tag, _customUrlProviderService));

            return View("Views/Admin/Tag/Index.cshtml", model);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View("Views/Admin/Tag/Create.cshtml");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AdminTagModel model)
        {
            // Check ModelState

            var tag = HorizontalMapper.MapTag(model);
            if (!String.IsNullOrEmpty(model.ArticleShortTitles))
                tag.Articles = _articleRepository.Articles.Where(x => model.ArticleShortTitles.Split(", ", StringSplitOptions.None).Contains(x.ShortTitle)).ToList();

            _tagRepository.CreateTag(tag);

            if (!String.IsNullOrEmpty(model.CustomUrl))
                _customUrlRepository.CreateMapping(new CustomUrl { NewUrl = model.CustomUrl, OriginalUrl = $"/Category/Tag?tagName={tag.Name}" });

            return Redirect("/admin/tag");
        }

        [HttpGet]
        [Route("detail")]
        public IActionResult Detail(int tagId)
        {
            var tag = _tagRepository.Tags.Where(x => x.Id == tagId).FirstOrDefault();
            if (tag == null)
                return NotFound();

            return View("Views/Admin/Tag/Detail.cshtml", HorizontalMapper.MapAdminTagModel(tag, _customUrlRepository));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AdminTagModel model)
        {
            var tag = _tagRepository.Tags.Where(x => x.Id == model.Id).FirstOrDefault();
            if (tag == null)
                return NotFound();
            tag = HorizontalMapper.MapTag(model, _articleRepository, tag);

            _tagRepository.SaveTag(tag);

            var customUrlMapping = _customUrlRepository.CustomUrls.Where(x => x.OriginalUrl == $"/Category/Tag?tagName={tag.Name}").FirstOrDefault();
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
                    OriginalUrl = $"/Category/Tag?tagName={tag.Name}",
                    NewUrl = model.CustomUrl
                };
                _customUrlRepository.CreateMapping(customUrlMapping);
            }

            return Redirect($"/admin/tag/detail?tagId={tag.Id}");
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete(int tagId)
        {
            var tag = _tagRepository.Tags.Where(x => x.Id == tagId).FirstOrDefault();
            if (tag == null)
                return NotFound();

            // Delete CustomUrl mapping if exists
            var customUrlMapping = _customUrlRepository.CustomUrls.Where(x => x.OriginalUrl == $"/Category/Tag?tagName={tag.Name}").FirstOrDefault();
            if (customUrlMapping != null)
                _customUrlRepository.DeleteMapping(customUrlMapping);

            if (tag.IsPublished)
            {
                tag.IsPublished = false;
                _tagRepository.SaveTag(tag);
            }
            else
            {
                _tagRepository.DeleteTag(tag);
            }

            return Redirect("/admin/tag");
        }
    }
}
