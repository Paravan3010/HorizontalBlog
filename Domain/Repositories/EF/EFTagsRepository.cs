using Horizontal.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Horizontal.Domain.Repositories.EF
{
    public class EFTagsRepository : ITagRepository
    {
        private HorizontalDbContext _context;

        public EFTagsRepository(HorizontalDbContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> Tags => _context.Tags.Include(x => x.Articles).ThenInclude(a => a.Tags);

        public int CreateTag(Tag tag)
        {
            _context.Add(tag);
            _context.SaveChanges();

            return tag.Id;
        }

        public void SaveTag(Tag tag)
        {
            _context.SaveChanges();
        }

        public void DeleteTag(Tag tag)
        {
            _context.Remove(tag);
            _context.SaveChanges();
        }
    }
}
