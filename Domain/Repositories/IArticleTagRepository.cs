namespace Horizontal.Domain.Repositories
{
    public interface IArticleTagRepository
    {
        public IQueryable<Article> GetArticlesByTag(Tag tag);
        public IQueryable<Tag> GetTagsByArticle(Article article);
        public void UpsertTagForArticle(Article article, Tag tag, int? order = null);
        public void RemoveTagFromArticle(Article article, Tag tag);
        public void SetTagsForArticle(Article article, params Tag[] tags);
        public void SetArticlesForTag(Tag tag, params Article[] articles);
    }
}
