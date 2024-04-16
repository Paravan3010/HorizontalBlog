using Horizontal.Domain.Repositories;
using Horizontal.Models.Interfaces;
using Horizontal.Services;

namespace Horizontal.Models
{
    public class MainModel : BaseModel, IPageableSite
    {
        public MainModel(INavigationService navigationService, 
                         ITagRepository tagRepository, 
                         ICategoryRepository categoryRepository) : 
            base(navigationService, tagRepository, categoryRepository)
        {
        }

        public IList<ArticleModel> Articles { get; set; } = new List<ArticleModel>();

        public int Page { get; set; } = 1;
        public int TotalNumberOfPages { get; set; }
        public string ControllerName { get; set; } = "Home";
        public string ActionName { get; set; }
        public List<(string, string)> RouteValues { get; set; } = new List<(string, string)>();
    }
}
