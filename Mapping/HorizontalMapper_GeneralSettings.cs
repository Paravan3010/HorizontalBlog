using Horizontal.Domain;
using Horizontal.Models.Admin;

namespace Horizontal.Mapping
{
    public static partial class HorizontalMapper
    {
        #region Admin

        public static GeneralSettingsModel MapGeneralSettingsModel(GeneralSettings domainModel)
        {
            return new GeneralSettingsModel()
            {
                Id = domainModel.Id,
                PageSize = domainModel.PageSize
            };
        }

        public static GeneralSettings MapGeneralSettings(GeneralSettingsModel viewModel, GeneralSettings domainModel = null)
        {
            var result = domainModel ?? new GeneralSettings();
            result.Id = viewModel.Id;
            result.PageSize = viewModel.PageSize;
            return result;
        }

        #endregion Admin
    }
}
