namespace Horizontal.Models.Admin
{
    public class AdminCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string ParentCategoryName { get; set; } = String.Empty;
        public bool IsPublished { get; set; } = false;
        public string ChildCategoryNames { get; set; } = String.Empty;
        public string ArticleShortNames { get; set; } = String.Empty;
        public string CustomUrl { get; set; } = String.Empty;
    }
}
