using Microsoft.EntityFrameworkCore;

namespace Horizontal.Domain.Contexts
{
    public class HorizontalDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<CustomUrl> CustomUrls => Set<CustomUrl>();

        public HorizontalDbContext(DbContextOptions<HorizontalDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                        .HasOne(c => c.ParentCategory)
                        .WithMany(c => c.ChildCategories);

            modelBuilder.Entity<Tag>()
                        .HasIndex(x => x.Name)
                        .IsUnique();

            modelBuilder.Entity<Article>()
                        .HasOne(a => a.NextArticle);

            modelBuilder.Entity<Article>()
                        .HasOne(a => a.PreviousArticle);
        }
    }
}
