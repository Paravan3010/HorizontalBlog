namespace Horizontal.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public Category ParentCategory { get; set; }
        public IList<Category> ChildCategories { get; set; } = new List<Category>();
        public IList<Article> Articles { get; set; } = new List<Article>();
        public bool IsPublished { get; set; } = false;
    }
}
