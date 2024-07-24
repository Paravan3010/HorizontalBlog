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
    public class HomeControllerTests
    {
        private readonly HomeController _controller;

        private readonly IGeneralSettingsRepository _generalSettingsRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArticleTagRepository _articleTagRepository;

        private readonly INavigationService _navigationService;

        public HomeControllerTests()
        {
            var context = InMemoryDbContextFactory.CreateHorizontalDbContext();

            // Repositories
            _articleRepository = new EFArticleRepository(context);
            _articleTagRepository = new EFArticleTagRepository(context);
            _categoryRepository = new EFCategoryRepository(context);
            _generalSettingsRepository = new EFGeneralSettingsRepository(context);
            _tagRepository = new EFTagsRepository(context);

            // Services
            _navigationService = new NavigationService(_categoryRepository, _articleRepository);

            // Controller
            _controller = new HomeController(_navigationService, _articleRepository, _tagRepository, 
                                             _categoryRepository, _generalSettingsRepository, _articleTagRepository);
        }

        [Fact]
        public void CanLoadMainPage()
        {
            // Arrange + Act
            var result = (_controller.Main(1) as ViewResult)?.ViewData.Model as MainModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            Assert.Equal(1, result.Page);
            Assert.Equal(2, result.TotalNumberOfPages);
            Assert.Equal(10, result.Articles.Count);

            var firstArticle = result.Articles.First();
            Assert.Equal("My travels through Vietnam: South to North by train, moped and bike", firstArticle.Title);
            Assert.Equal(new DateTime(2024, 5, 1), firstArticle.Published);
            Assert.Equal("travel", firstArticle.Tags.First(), true);
            Assert.Equal("nature", firstArticle.Tags.Skip(1).First(), true);
            Assert.Equal("history", firstArticle.Tags.Skip(2).First(), true);
        }

        [Fact]
        public void CannotLoadUnpublishedTagsOnArticlePreviews()
        {
            // Arrange
            var allPublishedTags = _tagRepository.Tags.Where(x => x.IsPublished).ToList();

            // Act
            var result = (_controller.Main(1) as ViewResult)?.ViewData.Model as MainModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            var firstArticle = result.Articles.First();
            Assert.Empty(firstArticle.Tags.Where(tag => tag.Equals("travel", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void CanNavigateMainPagePaging()
        {
            // Arrange + Act
            var page1 = (_controller.Main(1) as ViewResult)?.ViewData.Model as MainModel;
            if (page1 == null)
                page1 = new(_navigationService, _tagRepository, _categoryRepository);

            var page2 = (_controller.Main(2) as ViewResult)?.ViewData.Model as MainModel;
            if (page2 == null)
                page2 = new(_navigationService, _tagRepository, _categoryRepository);

            var page3 = (_controller.Main(3) as ViewResult)?.ViewData.Model as MainModel;
            if (page3 == null)
                page3 = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            //  - First page is full (contains 10 articles)
            Assert.Equal(1, page1.Page);
            Assert.Equal(2, page1.TotalNumberOfPages);
            Assert.Equal(10, page1.Articles.Count);
            //  - Second page is not full (contains 4 articles)
            Assert.Equal(2, page2.Page);
            Assert.Equal(2, page2.TotalNumberOfPages);
            Assert.Equal(4, page2.Articles.Count);
            //  - Third page is not present (contains 0 articles)
            Assert.Equal(3, page3.Page);
            Assert.Equal(0, page3.Articles.Count);
        }

        [Fact]
        public void CanGenerateTopNavBarLinks()
        {
            // Arrange + Act
            var result = (_controller.Main(1) as ViewResult)?.ViewData.Model as MainModel;
            if (result == null)
                result = new(_navigationService, _tagRepository, _categoryRepository);

            // Assert
            var topLinks = result.TopNavbarLinks.ToArray();
            //  - 8 Links (2 categories; 6 tags)
            Assert.Equal(8, topLinks.Length);
            Assert.Equal(2, topLinks.Where(x => x.Type == "Category").ToList().Count);
            Assert.Equal(6, topLinks.Where(x => x.Type == "Tag").ToList().Count);
            // Category Names
            Assert.Equal("nature", topLinks[0].Name, true);
            Assert.Equal("culture", topLinks[1].Name, true);
            Assert.Equal("general info", topLinks[2].Name, true);
            Assert.Equal("visa", topLinks[3].Name, true);
            Assert.Equal("history", topLinks[4].Name, true);
            Assert.Equal("trains", topLinks[5].Name, true);
            Assert.Equal("usa", topLinks[6].Name, true);
            Assert.Equal("east coast", topLinks[7].Name, true);
        }
    }
}