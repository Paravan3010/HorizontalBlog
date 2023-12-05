using Horizontal.Domain.Contexts;

namespace Horizontal.Domain.Repositories.EF
{
    public class EFCustomUrlRepository : ICustomUrlRepository
    {
        private HorizontalDbContext _context;

        public EFCustomUrlRepository(HorizontalDbContext context)
        {
            _context = context;
        }
        public IQueryable<CustomUrl> CustomUrls => _context.CustomUrls;

        public int CreateMapping(CustomUrl customUrl)
        {
            _context.Add(customUrl);
            _context.SaveChanges();

            return customUrl.Id;
        }

        public void SaveMapping(CustomUrl customUrl)
        {
            _context.SaveChanges();
        }

        public void DeleteMapping(CustomUrl customUrl)
        {
            _context.Remove(customUrl);
            _context.SaveChanges();
        }
    }
}
