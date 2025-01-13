using Horizontal.Domain.Repositories;
using Horizontal.Domain;
using Horizontal.Models;
using Horizontal.Services;
using Horizontal.Models.Admin;
using Horizontal.Controllers;

namespace Horizontal.Mapping
{
    public static partial class HorizontalMapper
    {
        public static CategoryModel MapCategoryModel(Category domainModel, INavigationService navService, ITagRepository tagRepo, ICategoryRepository categoryRepo)
        {
            var model = new CategoryModel(navService, tagRepo, categoryRepo)
            {
                CategoryId = domainModel.Id,
                CategoryName = domainModel.Name,
                ActionName = nameof(CategoryController.Category),
                PageTitle = domainModel.PageTitle,
                PageDescription = domainModel.PageDescription
            };
            model.RouteValues.Add(("categoryId", domainModel.Id.ToString()));

            return model;
        }

        public static CategoryModel MapCategoryModel(Tag domainModel, INavigationService navService, ITagRepository tagRepo, ICategoryRepository categoryRepo)
        {
            var model = new CategoryModel(navService, tagRepo, categoryRepo)
            {
                CategoryName = domainModel.Name,
                ActionName = nameof(CategoryController.Tag),
                PageTitle = domainModel.PageTitle,
                PageDescription = domainModel.PageDescription
            };
            model.RouteValues.Add(("tagName", domainModel.Name.ToString()));

            return model;
        }

        #region Admin   
        public static TagPreviewModel MapTagPreviewModel(Tag domainModel, ICustomUrlProviderService customUrlProvider)
        {
            return new TagPreviewModel
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                IsPublished = domainModel.IsPublished,
                IsInTopNavbar = domainModel.IsInTopNavbar,
                CustomUrl = customUrlProvider.HasCustomUrl("Category", "Tag", (key: "tagName", value: domainModel.Name)) ?
                            customUrlProvider.GetCustomUrl("Category", "Tag", (key: "tagName", value: domainModel.Name)) :
                            String.Empty
            };
        }

        public static AdminTagModel MapAdminTagModel(Tag domainModel, ICustomUrlRepository customUrlRepository, IArticleTagRepository articleTagRepository)
        {
            return new AdminTagModel()
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                PageTitle = domainModel.PageTitle ?? String.Empty,
                PageDescription = domainModel.PageDescription ?? String.Empty,
                IsPublished = domainModel.IsPublished,
                IsInTopNavbar = domainModel.IsInTopNavbar,
                TopNavbarOrder = domainModel.TopNavbarOrder,
                ArticleShortTitles = String.Join(Environment.NewLine, articleTagRepository.GetArticlesByTag(domainModel).Select(x => x.LongTitle) ?? Enumerable.Empty<string>()),
                CustomUrl = customUrlRepository.CustomUrls?
                                         .Where(x => x.OriginalUrl == $"/Category/Tag?tagName={domainModel.Name}")
                                         .FirstOrDefault()?.NewUrl ?? String.Empty
            };
        }

        public static Tag MapTag(AdminTagModel viewModel, IArticleTagRepository articleTagRepository, IArticleRepository articleRepository = null, Tag resultModel = null)
        {
            resultModel = resultModel ?? new Tag();
            resultModel.Name = viewModel.Name;
            resultModel.PageTitle = viewModel.PageTitle ?? String.Empty;
            resultModel.PageDescription = viewModel.PageDescription ?? String.Empty;
            resultModel.IsPublished = viewModel.IsPublished;
            resultModel.IsInTopNavbar = viewModel.IsInTopNavbar;
            resultModel.TopNavbarOrder = viewModel.TopNavbarOrder;
            if (!String.IsNullOrEmpty(viewModel.ArticleShortTitles))
            {
                articleTagRepository.SetArticlesForTag(resultModel, articleRepository.Articles.Where(x => viewModel.ArticleShortTitles
                                                                                    .Split(Environment.NewLine, StringSplitOptions.None)
                                                                                    .Select(x => x.Trim())
                                                                                    .Contains(x.LongTitle)).ToArray());
            }


            return resultModel;
        }

        public static CategoryPreviewModel MapCategoryPreviewModel(Category domainModel, ICustomUrlProviderService customUrlProvider)
        {
            return new CategoryPreviewModel
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                IsPublished = domainModel.IsPublished,
                CustomUrl = customUrlProvider.HasCustomUrl("Category", "Category", (key: "categoryId", value: domainModel.Id.ToString())) ?
                                customUrlProvider.GetCustomUrl("Category", "Category", (key: "categoryId", value: domainModel.Id.ToString())) :
                                String.Empty
            };
        }

        public static AdminCategoryModel MapAdminCategoryModel(Category domainModel, ICustomUrlRepository customUrlRepository)
        {
            return new AdminCategoryModel()
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                PageTitle = domainModel.PageTitle ?? String.Empty,
                PageDescription = domainModel.PageDescription ?? String.Empty,
                ParentCategoryName = domainModel?.ParentCategory?.Name ?? String.Empty,
                IsPublished = domainModel.IsPublished,
                IsInTopNavbar = domainModel.IsInTopNavbar,
                TopNavbarOrder = domainModel.TopNavbarOrder,
                GeneralOrder = domainModel.GeneralOrder,
                ChildCategoryNames = String.Join(", ", domainModel?.ChildCategories.Select(x => x.Name) ?? Enumerable.Empty<string>()),
                ArticleShortNames = String.Join(Environment.NewLine, domainModel?.Articles.Select(x => x.LongTitle) ?? Enumerable.Empty<string>()),
                CustomUrl = customUrlRepository.CustomUrls?.Where(x => x.OriginalUrl == $"/Category/Category?categoryId={domainModel.Id}").FirstOrDefault()?.NewUrl ?? String.Empty
            };
        }

        public static Category MapCategory(AdminCategoryModel viewModel, ICategoryRepository categoryRepository,
                                           IArticleRepository articleRepository, Category category = null)
        {
            category = category ?? new Category();
            category.Name = viewModel.Name;
            category.PageTitle = viewModel.PageTitle ?? String.Empty;
            category.PageDescription = viewModel.PageDescription ?? String.Empty;
            category.IsPublished = viewModel.IsPublished;
            category.IsInTopNavbar = viewModel.IsInTopNavbar;
            category.TopNavbarOrder = viewModel.TopNavbarOrder;
            category.GeneralOrder = viewModel.GeneralOrder;
            category.ParentCategory = categoryRepository.Categories.Where(x => x.Name == viewModel.ParentCategoryName).FirstOrDefault();
            if (!String.IsNullOrEmpty(viewModel.ChildCategoryNames))
                category.ChildCategories = categoryRepository.Categories.Where(x => viewModel.ChildCategoryNames.Split(", ", StringSplitOptions.None).Contains(x.Name)).ToList();
            if (!String.IsNullOrEmpty(viewModel.ArticleShortNames))
            {
                category.Articles = articleRepository.Articles.Where(x => viewModel.ArticleShortNames
                                                                                   .Split(Environment.NewLine, StringSplitOptions.None)
                                                                                   .Select(x => x.Trim())
                                                                                   .Contains(x.LongTitle))
                                                              .ToList();
            }

            return category;
        }
        #endregion Admin
    }
}