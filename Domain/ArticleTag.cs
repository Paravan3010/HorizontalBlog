using System.ComponentModel.DataAnnotations;

namespace Horizontal.Domain
{
    public class ArticleTag
    {
        public int ArticleId { get; set; }
        public Article? Article { get; set; }
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
        public int Order { get; set; }
    }
}
