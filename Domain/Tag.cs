namespace Horizontal.Domain
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public bool IsPublished { get; set; } = false;
        public bool IsInTopNavbar { get; set; } = false;
        public int TopNavbarOrder { get; set; }
    }
}