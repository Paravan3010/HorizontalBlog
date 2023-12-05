using Microsoft.EntityFrameworkCore;

namespace Horizontal.Domain
{
    [Index(nameof(NewUrl), IsUnique = true)]
    public class CustomUrl
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string NewUrl { get; set; } = String.Empty;
        public string OriginalUrl { get; set; } = String.Empty;
    }
}
