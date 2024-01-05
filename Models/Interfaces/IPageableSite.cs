namespace Horizontal.Models.Interfaces
{
    public interface IPageableSite
    {
        public int Page { get; set; }
        public int TotalNumberOfPages { get; set; }
    }
}
