using Horizontal.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Horizontal.Domain.Repositories.EF
{
    public class EFArticleRepository : IArticleRepository
    {
        private HorizontalDbContext _context;

        public EFArticleRepository(HorizontalDbContext context)
        {
            _context = context;
        }

        public IQueryable<Article> Articles => _context.Articles.Include(x => x.Category)
                                                                .Include(x => x.NextArticle)
                                                                .Include(x => x.PreviousArticle);

        public int UpsertArticle(Article article)
        {
            if (article.Id == 0)
                _context.Add(article);
            _context.SaveChanges();

            return article.Id;
        }

        public void SaveArticle(Article article)
        {
            _context.SaveChanges();
        }

        public void DeleteArticle(Article article)
        {
            _context.Remove(article);
            _context.SaveChanges();
        }

        public void IncreaseVisitCount(long articleId)
        {
            var article = Articles.FirstOrDefault(x => x.Id == articleId);
            if (article == null)
                return;
            article.NumberOfVisits++;
            _context.SaveChanges();
        }
    }
}
