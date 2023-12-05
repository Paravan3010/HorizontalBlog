namespace Horizontal.Models.Navigation
{
    public class CategoryNavigationModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public IList<CategoryNavigationModel> Subcategorie { get; set; }
        public IList<ArticleNavigationModel> Articles { get; set; }
    }
}
