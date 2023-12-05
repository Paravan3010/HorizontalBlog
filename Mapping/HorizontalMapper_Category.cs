using Horizontal.Domain.Repositories;
using Horizontal.Domain;
using Horizontal.Models;
using Horizontal.Services;
using Horizontal.Models.Admin;
using Horizontal.Domain.Repositories.EF;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Horizontal.Mapping
{
    public static partial class HorizontalMapper
    {
        public static CategoryModel MapCategoryModel(Category domainModel, INavigationService navService, ITagRepository tagRepo)
        {
            return new CategoryModel(navService, tagRepo)
            {
                CategoryId = domainModel.Id,
                CategoryName = domainModel.Name
            };
        }

        public static CategoryModel MapCategoryModel(Tag domainModel, INavigationService navService, ITagRepository tagRepo)
        {
            return new CategoryModel(navService, tagRepo)
            {
                CategoryName = domainModel.Name
            };
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

        public static AdminTagModel MapAdminTagModel(Tag domainModel, ICustomUrlRepository customUrlRepository)
        {
            return new AdminTagModel()
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                IsPublished = domainModel.IsPublished,
                IsInTopNavbar = domainModel.IsInTopNavbar,
                TopNavbarOrder = domainModel.TopNavbarOrder,
                ArticleShortTitles = String.Join(", ", domainModel?.Articles.Select(x => x.ShortTitle) ?? Enumerable.Empty<string>()),
                CustomUrl = customUrlRepository.CustomUrls?
                                         .Where(x => x.OriginalUrl == $"/Category/Tag?tagName={domainModel.Name}")
                                         .FirstOrDefault()?.NewUrl ?? String.Empty
            };
        }

        public static Tag MapTag(AdminTagModel viewModel, IArticleRepository articleRepository = null, Tag resultModel = null)
        {
            resultModel = resultModel ?? new Tag();
            resultModel.Name = viewModel.Name;
            resultModel.IsPublished = viewModel.IsPublished;
            resultModel.IsInTopNavbar = viewModel.IsInTopNavbar;
            resultModel.TopNavbarOrder = viewModel.TopNavbarOrder;
            if (String.IsNullOrEmpty(viewModel.ArticleShortTitles))
            { 
                resultModel.Articles = new List<Article>();
            }
            else
            {
                resultModel.Articles = articleRepository.Articles.Where(x => viewModel.ArticleShortTitles
                                                                                      .Split(", ", StringSplitOptions.None)
                                                                                      .Contains(x.ShortTitle)).ToList();
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
                ParentCategoryName = domainModel?.ParentCategory?.Name ?? String.Empty,
                IsPublished = domainModel.IsPublished,
                ChildCategoryNames = String.Join(", ", domainModel?.ChildCategories.Select(x => x.Name) ?? Enumerable.Empty<string>()),
                ArticleShortNames = String.Join(", ", domainModel?.Articles.Select(x => x.ShortTitle) ?? Enumerable.Empty<string>()),
                CustomUrl = customUrlRepository.CustomUrls?.Where(x => x.OriginalUrl == $"/Category/Category?categoryId={domainModel.Id}").FirstOrDefault()?.NewUrl ?? String.Empty
            };
        }

        public static Category MapCategory(AdminCategoryModel viewModel, ICategoryRepository categoryRepository, 
                                           IArticleRepository articleRepository, Category category = null)
        {
            category = category ?? new Category();
            category.Name = viewModel.Name;
            category.IsPublished = viewModel.IsPublished;
            if (!String.IsNullOrEmpty(viewModel.ParentCategoryName))
                category.ParentCategory = categoryRepository.Categories.Where(x => x.Name == viewModel.ParentCategoryName).FirstOrDefault();
            if (!String.IsNullOrEmpty(viewModel.ChildCategoryNames))
                category.ChildCategories = categoryRepository.Categories.Where(x => viewModel.ChildCategoryNames.Split(", ", StringSplitOptions.None).Contains(x.Name)).ToList();
            if (!String.IsNullOrEmpty(viewModel.ArticleShortNames))
                category.Articles = articleRepository.Articles.Where(x => viewModel.ArticleShortNames.Split(", ", StringSplitOptions.None).Contains(x.ShortTitle)).ToList();

            return category;
        }
        #endregion Admin
    }
}