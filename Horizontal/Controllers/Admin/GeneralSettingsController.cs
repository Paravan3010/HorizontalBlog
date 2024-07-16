using Horizontal.Domain.Repositories;
using Horizontal.Mapping;
using Horizontal.Models.Admin;
using Horizontal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Controllers.Admin
{
    [Authorize]
    [Route("admin/general")]
    public class GeneralSettingsController : Controller
    {
        private readonly INavigationService _navigationService;
        private readonly IGeneralSettingsRepository _generalSettingsRepository;

        public GeneralSettingsController(INavigationService navigationService, IGeneralSettingsRepository generalSettingsRepository)
        {
            _navigationService = navigationService;
            _generalSettingsRepository = generalSettingsRepository;
        }

        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var settings = _generalSettingsRepository.GeneralSettings.FirstOrDefault();
            if (settings == null)
            {
                settings = new Domain.GeneralSettings();
                _generalSettingsRepository.SaveGeneralSettings(settings);
            }

            return View("Views/Admin/GeneralSettings/Index.cshtml", HorizontalMapper.MapGeneralSettingsModel(settings));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(GeneralSettingsModel model)
        {
            var settings = _generalSettingsRepository.GeneralSettings.Where(x => x.Id == model.Id).First();
            _generalSettingsRepository.SaveGeneralSettings(HorizontalMapper.MapGeneralSettings(model, settings));

            return RedirectToAction("Index");
        }
    }
}
