using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using Horizontal.Models;
using Horizontal.Models.Navigation;
using System.Collections.Generic;

namespace Horizontal.Services.Implementation
{
    public class NavigationService : INavigationService
    {
        private static readonly object actualizationLock = new object();

        private IList<CategoryNavigationModel>? _categoryNavigation;

        private readonly ICategoryRepository _categoryRepository;
        private readonly IArticleRepository _articleRepository;

        public NavigationService(ICategoryRepository categoryRepository, IArticleRepository articleRepository)
        {
            _categoryRepository = categoryRepository;
            _articleRepository = articleRepository;
        }


        public IList<CategoryNavigationModel> GetCategoryNavigation()
        {
            if (_categoryNavigation == null)
                ActualizeCategoryNavigation();
#pragma warning disable CS8603 // Possible null reference return.
            return _categoryNavigation;
#pragma warning restore CS8603 // Possible null reference return.
        }


        public void ActualizeCategoryNavigation()
        {
            lock (actualizationLock)
            {
                _categoryNavigation = new List<CategoryNavigationModel>();

                var categories = _categoryRepository.Categories.Where(x => x.IsPublished);
                var articles = _articleRepository.Articles.Where(x => x.IsPublished);

                // Recursively add all categories and articles
                foreach (var topCategory in categories.Where(x => x.ParentCategory == null).OrderBy(o => o.Name))
                {
                    var cnm = new CategoryNavigationModel()
                    {
                        CategoryId = topCategory.Id,
                        Name = topCategory.Name,
                    };
                    _categoryNavigation.Add(cnm);
                    addCategoriesAndArticlesRec(cnm);
                }

                void addCategoriesAndArticlesRec(CategoryNavigationModel cnm)
                {
                    var subcategories = categories.Where(x => x.ParentCategory != null && x.ParentCategory.Id == cnm.CategoryId).OrderBy(o => o.Name);
                    if (subcategories.Any())
                        cnm.Subcategorie = new List<CategoryNavigationModel>();
                    foreach (var subcategory in subcategories)
                    {
                        var subcategoryCNM = new CategoryNavigationModel()
                        {
                            CategoryId = subcategory.Id,
                            Name = subcategory.Name,
                        };
                        cnm.Subcategorie?.Add(subcategoryCNM);
                        addCategoriesAndArticlesRec(subcategoryCNM);
                    }

                    var subcatArticles = articles.Where(x => x.Category != null && x.Category.Id == cnm.CategoryId).OrderBy(o => o.Order);
                    if (subcatArticles.Any())
                        cnm.Articles = new List<ArticleNavigationModel>();
                    foreach (var article in subcatArticles)
                    {
                        cnm.Articles?.Add(new ArticleNavigationModel()
                        {
                            ArticleId = article.Id,
                            Name = article.ShortTitle
                        });
                    }
                }
            }
        }
    }
}
