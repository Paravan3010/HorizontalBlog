namespace Horizontal.Domain.Repositories
{
    public interface IArticleRepository
    {
        IQueryable<Article> Articles { get; }

        int UpsertArticle(Article article);
        void SaveArticle(Article article);
        void DeleteArticle(Article article);
        void IncreaseVisitCount(long articleId);
    }
}
