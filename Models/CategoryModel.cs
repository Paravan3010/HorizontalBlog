using Horizontal.Domain.Repositories;
using Horizontal.Services;

namespace Horizontal.Models
{
    public class CategoryModel : BaseModel
    {
        public CategoryModel(INavigationService navigationService, ITagRepository tagRepository) : base(navigationService, tagRepository)
        {
        }

        public int? CategoryId { get; set; } = null;
        public string CategoryName { get; set; } = String.Empty;
        public IList<ArticleModel> Articles { get; set; } = new List<ArticleModel>();
    }
}
