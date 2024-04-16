namespace Horizontal.Models.Admin
{
    public class AdminTagModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string PageTitle { get; set; } = String.Empty;
        public string PageDescription { get; set; } = String.Empty;
        public bool IsPublished { get; set; } 
        public bool IsInTopNavbar { get; set; }
        public int TopNavbarOrder { get; set; } = 0;
        public string ArticleShortTitles { get; set; } = String.Empty;
        public string CustomUrl { get; set; } = String.Empty;
    }
}
