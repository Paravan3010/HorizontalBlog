using Horizontal.Domain.Repositories;
using Horizontal.Models.Navigation;
using Horizontal.Services;

namespace Horizontal.Models
{
    public class MainModel : BaseModel
    {
        public MainModel(INavigationService navigationService, ITagRepository tagRepository) : base(navigationService, tagRepository)
        {
        }

        public IList<ArticleModel> Articles { get; set; } = new List<ArticleModel>();
    }
}
