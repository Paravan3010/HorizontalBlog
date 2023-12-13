using Horizontal.Domain.Repositories;
using Horizontal.Services;

namespace Horizontal.Models
{
    public class ArticleModel : BaseModel
    {
        public ArticleModel(INavigationService navigationService, ITagRepository tagRepository) : base(navigationService, tagRepository)
        {
        }

        public int Id { get; set; }
        public string PreviewPhotoPath { get; set; } = String.Empty;
        public IList<string> Tags { get; set; } = new List<string>();
        public string Title { get; set; } = String.Empty;
        public DateTime Published { get; set; }
        public DateTime LastUpdated { get; set; }
        public string TextBeginning { get; set; } = String.Empty;
        public string ArticleHtmlPath { get; set; } = String.Empty;
        public bool GalleryExists { get; set; } = false;

        public int? NextArticleId { get; set; }
        public string NextArticleShortName { get; set; } = String.Empty;
        public bool IsNextArticlePublished { get; set; }

        public int? PreviousArticleId { get; set; }
        public string PreviousArticleShortName { get; set; } = String.Empty;
        public bool IsPreviousArticlePublished { get; set; }
    }
}
