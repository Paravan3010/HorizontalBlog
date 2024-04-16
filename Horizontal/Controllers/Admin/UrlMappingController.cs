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
    [Route("admin/url-mapping")]
    public class UrlMappingController : Controller
    {
        private ICustomUrlRepository _customUrlRepository;


        public UrlMappingController(ICustomUrlRepository customUrlRepository)
        {
            _customUrlRepository = customUrlRepository;
        }

        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var model = new List<UrlMappingModel>();
            foreach (var mapping in _customUrlRepository.CustomUrls.OrderBy(x => x.OriginalUrl))
                model.Add(HorizontalMapper.MapUrlMappingModel(mapping));

            return View("Views/Admin/UrlMapping/Index.cshtml", model);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View("Views/Admin/UrlMapping/Create.cshtml");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(UrlMappingModel model)
        {
            // Check ModelState

            _customUrlRepository.CreateMapping(HorizontalMapper.MapCustomUrl(model));

            return Redirect("/admin/url-mapping");
        }

        [HttpGet]
        [Route("detail")]
        public IActionResult Detail(int customUrlId)
        {
            var customUrl = _customUrlRepository.CustomUrls.Where(x => x.Id == customUrlId).FirstOrDefault();
            if (customUrl == null)
                return NotFound();

            return View("Views/Admin/UrlMapping/Detail.cshtml", HorizontalMapper.MapUrlMappingModel(customUrl));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(UrlMappingModel model)
        {
            var customUrl = _customUrlRepository.CustomUrls.Where(x => x.Id == model.Id).FirstOrDefault();
            if (customUrl == null)
                return NotFound();

            customUrl = HorizontalMapper.MapCustomUrl(model, customUrl);
            _customUrlRepository.SaveMapping(customUrl);

            return Redirect($"/admin/url-mapping/detail?customUrlId={customUrl.Id}");
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete(int customUrlId)
        {
            var customUrl = _customUrlRepository.CustomUrls.Where(x => x.Id == customUrlId).FirstOrDefault();
            if (customUrl == null)
                return NotFound();

            if (customUrl.IsActive)
            {
                customUrl.IsActive = false;
                _customUrlRepository.SaveMapping(customUrl);
            }
            else
            {
                _customUrlRepository.DeleteMapping(customUrl);
            }

            return Redirect("/admin/url-mapping");
        }
    }
}
