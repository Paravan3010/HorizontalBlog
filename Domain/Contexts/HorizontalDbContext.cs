﻿using Microsoft.EntityFrameworkCore;

namespace Horizontal.Domain.Contexts
{
    public class HorizontalDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<ArticleTag> ArticleTags => Set<ArticleTag>();
        public DbSet<CustomUrl> CustomUrls => Set<CustomUrl>();
        public DbSet<GeneralSettings> GeneralSettings => Set<GeneralSettings>();

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
            
            modelBuilder.Entity<ArticleTag>().ToTable("ArticleTag");
            modelBuilder.Entity<ArticleTag>()
                        .HasKey(at => new { at.ArticleId, at.TagId });
        }
    }
}
