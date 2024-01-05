using Horizontal.Domain.Repositories;
using Horizontal.Models.Interfaces;
using Horizontal.Services;

namespace Horizontal.Models
{
    public class MainModel : BaseModel, IPageableSite
    {
        public MainModel(INavigationService navigationService, ITagRepository tagRepository) : base(navigationService, tagRepository)
        {
        }

        public IList<ArticleModel> Articles { get; set; } = new List<ArticleModel>();

        public int Page { get; set; } = 1;
        public int TotalNumberOfPages { get; set; }
    }
}
