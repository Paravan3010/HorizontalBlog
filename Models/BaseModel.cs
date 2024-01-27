using Horizontal.Domain.Repositories;
using Horizontal.Models.Admin;
using Horizontal.Models.Navigation;
using Horizontal.Services;
using System.Linq;

namespace Horizontal.Models
{
    public abstract class BaseModel
    {
        public BaseModel(INavigationService navigationService, ITagRepository tagRepository, ICategoryRepository categoryRepository) 
        {
            var tagQuery = tagRepository.Tags
               .Where(x => x.IsInTopNavbar)
               .Select(x => new TopNavBarLink { Name = x.Name, TopNavbarOrder = x.TopNavbarOrder, Type = "Tag" })
               .ToList();

            var categoryQuery = categoryRepository.Categories
                .Where(x => x.IsInTopNavbar)
                .Select(x => new TopNavBarLink { Name = x.Name, TopNavbarOrder = x.TopNavbarOrder, Type = "Category", CategoryId = x.Id })
                .ToList();

            TopNavbarLinks = tagQuery
                .Union(categoryQuery)
                .OrderBy(x => x.TopNavbarOrder)
                .ThenBy(x => x.Name)
                .Select(x => new TopNavbarLinkModel() { Type = x.Type, Name = x.Name, CategoryId = x.CategoryId })
                .ToList();

            CategoriesNavigation = navigationService.GetCategoryNavigation();
        }

        public IList<TopNavbarLinkModel> TopNavbarLinks { get; set; }
        public IList<CategoryNavigationModel> CategoriesNavigation { get; set; }

        private class TopNavBarLink
        {
            /// <summary>
            /// Differentiates tags from category links
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Link name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Link order in top bar
            /// </summary>
            public int TopNavbarOrder { get; set; }

            /// <summary>
            /// Id for link of type Category
            /// </summary>
            public int CategoryId { get; set; }
        }
    }
}
