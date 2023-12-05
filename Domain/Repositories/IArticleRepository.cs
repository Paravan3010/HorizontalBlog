namespace Horizontal.Domain.Repositories
{
    public interface IArticleRepository
    {
        IQueryable<Article> Articles { get; }

        int CreateArticle(Article article);
        void SaveArticle(Article article);
        void DeleteArticle(Article article);
        void IncreaseVisitCount(long articleId);
    }
}
