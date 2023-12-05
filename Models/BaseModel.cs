using Horizontal.Domain.Repositories;
using Horizontal.Models.Navigation;
using Horizontal.Services;

namespace Horizontal.Models
{
    public abstract class BaseModel
    {
        public BaseModel(INavigationService navigationService, ITagRepository tagRepository) 
        {
            TopNavbarTags = tagRepository.Tags.Where(x => x.IsInTopNavbar)
                                              .OrderBy(x => x.TopNavbarOrder)
                                              .ThenBy(x => x.Name)
                                              .Select(x => x.Name)
                                              .ToList();
            CategoriesNavigation = navigationService.GetCategoryNavigation();
        }

        public IList<string> TopNavbarTags { get; set; }
        public IList<CategoryNavigationModel> CategoriesNavigation { get; set; }
    }
}
