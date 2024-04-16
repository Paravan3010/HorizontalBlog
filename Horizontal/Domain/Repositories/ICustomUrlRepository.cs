namespace Horizontal.Domain.Repositories
{
    public interface ICustomUrlRepository
    {
        IQueryable<CustomUrl> CustomUrls { get; }

        int CreateMapping(CustomUrl customUrl);
        void SaveMapping(CustomUrl customUrl);
        void DeleteMapping(CustomUrl customUrl);
    }
}
