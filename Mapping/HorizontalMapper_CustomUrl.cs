using Horizontal.Domain;
using Horizontal.Models.Admin;

namespace Horizontal.Mapping
{
    public static partial class HorizontalMapper
    {
        #region Admin
        public static UrlMappingModel MapUrlMappingModel(CustomUrl domainModel)
        {
            return new UrlMappingModel()
            {
                Id = domainModel.Id,
                IsActive = domainModel.IsActive,
                OriginalUrl = domainModel.OriginalUrl,
                NewUrl = domainModel.NewUrl
            };
        }

        public static CustomUrl MapCustomUrl(UrlMappingModel viewModel, CustomUrl resultModel = null)
        {
            resultModel = resultModel ?? new CustomUrl();
            resultModel.IsActive = viewModel.IsActive;
            resultModel.OriginalUrl = viewModel.OriginalUrl;
            resultModel.NewUrl = viewModel.NewUrl;

            return resultModel;
        }
        #endregion Admin
    }
}