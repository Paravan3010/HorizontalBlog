using Horizontal.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Horizontal.Domain.Repositories.EF
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private HorizontalDbContext _context;

        public EFCategoryRepository(HorizontalDbContext context)
        {
            _context = context;
        }


        public IQueryable<Category> Categories => _context.Categories.Include(x => x.ParentCategory).Include(x => x.ChildCategories).Include(x => x.Articles);

        public int CreateCategory(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();

            return category.Id;
        }
        public void SaveCategory(Category category)
        {
            _context.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            _context.Remove(category);
            _context.SaveChanges();
        }
    }
}
