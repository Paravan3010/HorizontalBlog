namespace Horizontal.Domain
{
    public class GeneralSettings
    {
        public int Id { get; set; }
        public int PageSize { get; set; } = 10;
        public string MainPageTitle { get; set; } = String.Empty;
        public string MainPageDescription { get; set; } = String.Empty;
    }
}
