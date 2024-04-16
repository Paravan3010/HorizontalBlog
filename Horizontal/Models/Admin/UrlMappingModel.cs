namespace Horizontal.Models.Admin
{
    public class UrlMappingModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string OriginalUrl { get; set; } = String.Empty;
        public string NewUrl { get; set; } = String.Empty;
    }
}
