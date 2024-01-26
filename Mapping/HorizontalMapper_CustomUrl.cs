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
                OriginalUrl = domainModel.OriginalUrl ?? String.Empty,
                NewUrl = domainModel.NewUrl ?? String.Empty
            };
        }

        public static CustomUrl MapCustomUrl(UrlMappingModel viewModel, CustomUrl resultModel = null)
        {
            resultModel = resultModel ?? new CustomUrl();
            resultModel.IsActive = viewModel.IsActive;
            resultModel.OriginalUrl = viewModel.OriginalUrl ?? String.Empty;
            resultModel.NewUrl = viewModel.NewUrl ?? String.Empty;

            return resultModel;
        }
        #endregion Admin
    }
}