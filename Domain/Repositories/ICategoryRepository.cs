namespace Horizontal.Domain.Repositories
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }

        int CreateCategory(Category category);
        void SaveCategory(Category category);
        void DeleteCategory(Category category);
    }
}
