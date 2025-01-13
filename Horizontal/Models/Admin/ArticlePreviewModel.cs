namespace Horizontal.Models.Admin
{
    public class ArticlePreviewModel
    {
        public int Id { get; set; }
        public string ShortTitle { get; set; }
        public bool IsPublished { get; set; }
        public bool IsInFeed { get; set; }
        public string CustomUrl { get; set; }
        public int NumberOfVisits { get; set; }
    }
}
