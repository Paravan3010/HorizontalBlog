namespace Horizontal.Models.Admin
{
    public class TagPreviewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPublished { get; set; }
        public bool IsInTopNavbar { get; set; }
        public string CustomUrl { get; set; }
    }
}
