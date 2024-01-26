namespace Horizontal.Models.Interfaces
{
    public interface IPageableSite
    {
        public int Page { get; set; }
        public int TotalNumberOfPages { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}
