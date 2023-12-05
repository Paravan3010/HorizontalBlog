namespace Horizontal.Models.Admin
{
    public class AdminArticleModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ShortTitle { get; set; } = String.Empty;
        public string LongTitle { get; set; } = String.Empty;
        public string TextBeginning { get; set; } = String.Empty;
        public int Order { get; set; }
        public string Tags { get; set; } = String.Empty;
        public string FilePath { get; set; } = String.Empty;
        public string PreviewPhotoPath { get; set; } = String.Empty;
        public string Published { get; set; } = String.Empty;
        public string LastUpdated { get; set; } = String.Empty;
        public string CustomUrl { get; set; } = String.Empty;
        public string GalleryUrl { get; set; } = String.Empty;
        public bool IsPublished { get; set; }
        public int? NextArticleId { get; set; }
        public int? PreviousArticleId { get; set; }
        public List<ArticleDropdownModel> AllArticles = new List<ArticleDropdownModel>();
    }

    public class ArticleDropdownModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}