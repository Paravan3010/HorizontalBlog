using Horizontal.Domain.Contexts;
using Horizontal.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Horizontal.Domain.Repositories.EF
{
    public class EFArticleTagRepository : IArticleTagRepository
    {
        private HorizontalDbContext _context;

        public EFArticleTagRepository(HorizontalDbContext context)
        {
            _context = context;
        }
        private IQueryable<ArticleTag> ArticleTags => _context.ArticleTags.Include(x => x.Article).Include(x => x.Tag);


        public void UpsertTagForArticle(Article article, Tag tag, int? order = null)
        {
            var articleTag = ArticleTags.Where(x => x.Tag.Name == tag.Name && x.Article.Id == article.Id).FirstOrDefault();
            if (articleTag != null && order != null)
            {
                articleTag.Order = (int)order;
                _context.SaveChanges();
                return;
            }

            // Both tag and article must be saved before setting article-tag mapping
            if (article.Id == 0)
            {
                _context.Add(article);
                _context.SaveChanges();
            }
            if (tag.Id == 0)
            {
                _context.Add(tag);
                _context.SaveChanges();
            }

            var newArticleTag = new ArticleTag()
            {
                Article = article,
                Tag = tag,
                Order = order ?? ArticleTags.Where(x => x.Article.Id == article.Id).Max(x => x.Order) + 1
            };
            _context.Add(newArticleTag);
            _context.SaveChanges();
        }

        public IQueryable<Article> GetArticlesByTag(Tag tag)
        {
            return ArticleTags.Where(x => x.Tag.Name == tag.Name).Select(x => x.Article);
        }

        public IQueryable<Tag> GetTagsByArticle(Article article)
        {
            return ArticleTags.Where(x => x.Article.Id == article.Id).OrderBy(x => x.Order).Select(x => x.Tag);
        }

        public void RemoveTagFromArticle(Article article, Tag tag)
        {
            var articleTag = ArticleTags.Where(x => x.Article.Id == article.Id && x.Tag.Name == tag.Name).FirstOrDefault();
            if (articleTag == null)
                return;
            _context.Remove(articleTag);
            _context.SaveChanges();
        }

        public void SetArticlesForTag(Tag tag, params Article[] articles)
        {
            foreach (var articleTagsToDelete in ArticleTags.Where(x => x.Tag.Name == tag.Name))
                _context.Remove(articleTagsToDelete);

            int counter = 0;
            foreach(var article in articles)
            {
                _context.Add(new ArticleTag()
                {
                    Article = article,
                    Tag = tag,
                    Order = ++counter
                });
            }

            _context.SaveChanges();
        }

        public void SetTagsForArticle(Article article, params Tag[] tags)
        {
            foreach (var articleTagsToDelete in ArticleTags.Where(x => x.Article.Id == article.Id))
                _context.Remove(articleTagsToDelete);

            int counter = 0;
            foreach (var tag in tags)
            {
                _context.Add(new ArticleTag()
                {
                    Article = article,
                    Tag = tag,
                    Order = ++counter
                });
            }

            _context.SaveChanges();
        }
    }
}
