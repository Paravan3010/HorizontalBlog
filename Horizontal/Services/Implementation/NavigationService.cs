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

        private IList<CategoryNavigationModel> _categoryNavigation;
        private IServiceProvider _services { get; }

        public NavigationService(IServiceProvider services)
        {
            _services = services;
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

                // The NavigationService is used as a singleton; therefore, to access DB data, a scope must be created.
                using var scope = _services.CreateScope();
                var scopedCategoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
                var scopedArticleRepository = scope.ServiceProvider.GetRequiredService<IArticleRepository>();
                var categories = scopedCategoryRepository.Categories.Where(x => x.IsPublished);
                var articles = scopedArticleRepository.Articles.Where(x => x.IsPublished);

                // Recursively add all categories and articles
                foreach (var topCategory in categories.Where(x => x.ParentCategory == null)
                                                      .OrderBy(o => o.GeneralOrder)
                                                      .ThenBy(o => o.Name))
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
                    var subcategories = categories.Where(x => x.ParentCategory != null && x.ParentCategory.Id == cnm.CategoryId)
                                                  .OrderBy(o => o.GeneralOrder)
                                                  .ThenBy(o => o.Name);
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
