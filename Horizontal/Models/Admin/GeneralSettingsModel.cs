namespace Horizontal.Models.Admin
{
    public class GeneralSettingsModel
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public string MainPageTitle { get; set; }
        public string MainPageDescription { get; set; }
        public string InstagramUrl { get; set; }
    }
}
