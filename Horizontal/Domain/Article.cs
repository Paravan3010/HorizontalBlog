namespace Horizontal.Domain
{
    public class Article
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string FilePath { get; set; } = String.Empty;
        public string PreviewPhotoPath { get; set; } = String.Empty;
        public string ShortTitle { get; set; } = String.Empty;
        public string LongTitle { get; set; } = String.Empty;
        public string TextBeginning { get; set; } = String.Empty;
        public string PageTitle { get; set; } = String.Empty;
        public string PageDescription { get; set; } = String.Empty;
        public Category? Category { get; set; }
        public DateTime Created { get; set; } = DateTime.Today;
        public DateTime LastUpdated { get; set; } = DateTime.Today;
        public bool IsPublished { get; set; } = false;
        public bool IsInFeed { get; set; } = true;
        public string GalleryUrl { get; set; } = String.Empty;
        public int NumberOfVisits { get; set; }
        public Article? PreviousArticle { get; set; }
        public Article? NextArticle { get; set; }
    }
}
