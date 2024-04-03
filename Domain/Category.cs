namespace Horizontal.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string PageTitle { get; set; } = String.Empty;
        public string PageDescription { get; set; } = String.Empty;
        public Category? ParentCategory { get; set; }
        public IList<Category> ChildCategories { get; set; } = new List<Category>();
        public IList<Article> Articles { get; set; } = new List<Article>();
        public bool IsInTopNavbar { get; set; } = false;
        public int TopNavbarOrder { get; set; }
        public bool IsPublished { get; set; } = false;
    }
}
