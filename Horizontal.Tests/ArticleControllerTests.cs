using Horizontal.Controllers;
using Horizontal.Domain.Repositories;
using Horizontal.Domain.Repositories.EF;
using Horizontal.Models;
using Horizontal.Services;
using Horizontal.Services.Implementation;
using Horizontal.Tests.Mocks.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Horizontal.Tests
{
    public class ArticleControllerTests
    {
        private readonly ArticleController _controller;

        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArticleTagRepository _articleTagRepository;

        private readonly INavigationService _navigationService;

        public ArticleControllerTests()
        {
            var context = InMemoryDbContextFactory.CreateHorizontalDbContext();

            // Repositories
            _articleRepository = new EFArticleRepository(context);
            _tagRepository = new EFTagsRepository(context);
            _categoryRepository = new EFCategoryRepository(context);
            _articleTagRepository = new EFArticleTagRepository(context);

            // Services
            _navigationService = new NavigationService(_categoryRepository, _articleRepository);

            // Controller
            _controller = new ArticleController(_articleRepository, _navigationService, _tagRepository, 
                                                _categoryRepository, _articleTagRepository);
        }

        [Fact]
        public void CanLoadFullArticle()
        {
            // Arrange
            var articleTitle = "My travels through Vietnam: South to North by train, moped and bike";
            var article = _articleRepository.Articles.Where(x => x.LongTitle == articleTitle).FirstOrDefault();

            // Arrange + Act
            var result = (_controller.FullArticle(article?.Id ?? 0) as ViewResult)?.ViewData.Model as ArticleModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            Assert.NotNull(article);
            Assert.True(!String.IsNullOrEmpty(result.ArticleHtmlPath));
            Assert.Equal(articleTitle, result.Title);
            Assert.Equal(new DateTime(2024, 5, 1), result.Published);
            Assert.Equal(new DateTime(2024, 5, 8), result.LastUpdated);
        }

        [Fact]
        public void CanLoadArticleTags()
        {
            // Arrange
            var articleTitle = "My travels through Vietnam: South to North by train, moped and bike";
            var article = _articleRepository.Articles.Where(x => x.LongTitle == articleTitle).FirstOrDefault();

            var allPublishedTags = _tagRepository.Tags.Where(x => x.IsPublished).ToList();

            // Arrange + Act
            var result = (_controller.FullArticle(article?.Id ?? 0) as ViewResult)?.ViewData.Model as ArticleModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            Assert.NotNull(article);
            // - Tags are loaded
            Assert.Equal(4, result.Tags.Count);
            Assert.Equal("travel", result.Tags.First(), true);
            Assert.Equal("nature", result.Tags.Skip(1).First(), true);
            Assert.Equal("history", result.Tags.Skip(2).First(), true);
            Assert.Equal("culture", result.Tags.Skip(3).First(), true);
            // - Tags exist
            Assert.Collection(allPublishedTags, tag => Assert.Contains(result.Tags.First(), tag.Name));
            Assert.Collection(allPublishedTags, tag => Assert.Contains(result.Tags.Skip(1).First(), tag.Name));
            Assert.Collection(allPublishedTags, tag => Assert.Contains(result.Tags.Skip(2).First(), tag.Name));
            Assert.Collection(allPublishedTags, tag => Assert.Contains(result.Tags.Skip(3).First(), tag.Name));
        }

        [Fact]
        public void CannotLoadNonexistingNextPreviousNavigation()
        {
            // Arrange
            var articleTitle = "My travels through Vietnam: South to North by train, moped and bike";
            var article = _articleRepository.Articles.Where(x => x.LongTitle == articleTitle).FirstOrDefault();

            // Arrange + Act
            var result = (_controller.FullArticle(article?.Id ?? 0) as ViewResult)?.ViewData.Model as ArticleModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            Assert.NotNull(article);
            Assert.Null(result.NextArticleId);
            Assert.True(String.IsNullOrEmpty(result.NextArticleLongTitle));
            Assert.False(result.IsNextArticlePublished);

            Assert.Null(result.PreviousArticleId);
            Assert.True(String.IsNullOrEmpty(result.PreviousArticleLongTitle));
            Assert.False(result.IsPreviousArticlePublished);
        }

        [Fact]
        public void CanLoadOnlyNextNavigationUnpublished()
        {
            // Arrange
            var articleTitle = "Schengen area overview";
            var article = _articleRepository.Articles.Where(x => x.LongTitle == articleTitle).FirstOrDefault();

            // Arrange + Act
            var result = (_controller.FullArticle(article?.Id ?? 0) as ViewResult)?.ViewData.Model as ArticleModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            Assert.NotNull(article);
            Assert.NotNull(result.NextArticleId);
            Assert.Equal("Visa to the UK", result.NextArticleLongTitle);
            // The next article is unpublished
            Assert.False(result.IsNextArticlePublished);    

            Assert.Null(result.PreviousArticleId);
            Assert.True(String.IsNullOrEmpty(result.PreviousArticleLongTitle));
            Assert.False(result.IsPreviousArticlePublished);
        }

        [Fact]
        public void CanLoadOnlyNextNavigation()
        {
            // Arrange
            var articleTitle = "How I visited the Great Wall of China from Beijing";
            var article = _articleRepository.Articles.Where(x => x.LongTitle == articleTitle).FirstOrDefault();

            // Arrange + Act
            var result = (_controller.FullArticle(article?.Id ?? 0) as ViewResult)?.ViewData.Model as ArticleModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            Assert.NotNull(article);
            Assert.NotNull(result.NextArticleId);
            Assert.Equal("How to use trains in China", result.NextArticleLongTitle);
            Assert.True(result.IsNextArticlePublished);

            Assert.Null(result.PreviousArticleId);
            Assert.True(String.IsNullOrEmpty(result.PreviousArticleLongTitle));
            Assert.False(result.IsPreviousArticlePublished);
        }

        [Fact]
        public void CanLoadOnlyPreviousNavigation()
        {
            // Arrange
            var articleTitle = "How to use trains in India";
            var article = _articleRepository.Articles.Where(x => x.LongTitle == articleTitle).FirstOrDefault();

            // Arrange + Act
            var result = (_controller.FullArticle(article?.Id ?? 0) as ViewResult)?.ViewData.Model as ArticleModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            Assert.NotNull(article);
            Assert.Null(result.NextArticleId);
            Assert.True(String.IsNullOrEmpty(result.NextArticleLongTitle));
            Assert.False(result.IsNextArticlePublished);

            Assert.NotNull(result.PreviousArticleId);
            Assert.Equal("How to use trains in China", result.PreviousArticleLongTitle);
            Assert.True(result.IsPreviousArticlePublished);
        }

        [Fact]
        public void CanLoadOnlyNextPreviousNavigation()
        {
            // Arrange
            var articleTitle = "How to use trains in China";
            var article = _articleRepository.Articles.Where(x => x.LongTitle == articleTitle).FirstOrDefault();

            // Arrange + Act
            var result = (_controller.FullArticle(article?.Id ?? 0) as ViewResult)?.ViewData.Model as ArticleModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            Assert.NotNull(article);
            Assert.NotNull(result.NextArticleId);
            Assert.Equal("How to use trains in India", result.NextArticleLongTitle);
            Assert.True(result.IsNextArticlePublished);

            Assert.NotNull(result.PreviousArticleId);
            Assert.Equal("How I visited the Great Wall of China from Beijing", result.PreviousArticleLongTitle);
            Assert.True(result.IsPreviousArticlePublished);
        }
    }
}