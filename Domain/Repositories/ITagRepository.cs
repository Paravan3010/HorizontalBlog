namespace Horizontal.Domain.Repositories
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }

        int CreateTag(Tag tag);
        void SaveTag(Tag tag);
        void DeleteTag(Tag tag);
    }
}
