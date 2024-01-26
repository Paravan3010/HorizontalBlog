using Horizontal.Domain.Repositories;
using Horizontal.Models.Interfaces;
using Horizontal.Services;

namespace Horizontal.Models
{
    public class CategoryModel : BaseModel, IPageableSite
    {
        public CategoryModel(INavigationService navigationService, ITagRepository tagRepository) : base(navigationService, tagRepository)
        {
        }

        public int? CategoryId { get; set; } = null;
        public string CategoryName { get; set; } = String.Empty;
        public IList<ArticleModel> Articles { get; set; } = new List<ArticleModel>();
        public int Page { get; set; } = 1;
        public int TotalNumberOfPages { get; set; }
        public string ControllerName { get; set; } = "Category";
        public string ActionName { get; set; }
        public List<(string, string)> RouteValues { get; set; } = new List<(string, string)>();
    }
}
