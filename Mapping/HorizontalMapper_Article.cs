using Horizontal.Domain.Repositories;
using Horizontal.Domain;
using Horizontal.Models;
using Horizontal.Services;
using Horizontal.Models.Admin;
using Horizontal.Domain.Repositories.EF;

namespace Horizontal.Mapping
{
    public static partial class HorizontalMapper
    {
        #region Admin
        public static Article MapArticle(AdminArticleModel viewModel, ICategoryRepository categoryRepository,
                                         ITagRepository tagRepository, IArticleRepository articleRepository, Article resultModel = null)
        {
            resultModel = resultModel ?? new Article();
            resultModel.Order = viewModel.Order;
            resultModel.FilePath = viewModel.FilePath;
            resultModel.PreviewPhotoPath = viewModel.PreviewPhotoPath;
            resultModel.ShortTitle = viewModel.ShortTitle;
            resultModel.LongTitle = viewModel.LongTitle;
            resultModel.TextBeginning = viewModel.TextBeginning;
            resultModel.Category = categoryRepository.Categories.Where(x => x.Name == viewModel.CategoryName).FirstOrDefault();
            resultModel.Created = DateTime.Parse(viewModel.Published);
            resultModel.LastUpdated = DateTime.Parse(viewModel.LastUpdated);
            resultModel.IsPublished = viewModel.IsPublished;
            resultModel.GalleryUrl = viewModel.GalleryUrl;
            resultModel.PreviousArticle = articleRepository.Articles.Where(x => x.Id == viewModel.PreviousArticleId).FirstOrDefault();
            resultModel.NextArticle = articleRepository.Articles.Where(x => x.Id == viewModel.NextArticleId).FirstOrDefault();

            foreach (var tagName in viewModel.Tags?.Split(", ") ?? Enumerable.Empty<string>())
            {
                if (resultModel.Tags.Any(x => x.Name == tagName))
                    continue;
                resultModel.Tags.Add(tagRepository.Tags.Where(x => x.Name == tagName).First());
            }
            foreach (var tag in resultModel.Tags)
            {
                if (viewModel.Tags.Split(", ").Contains(tag.Name))
                    continue;
                resultModel.Tags.Remove(tag);
            }

            return resultModel;
        }

        public static ArticleModel MapArticleModel(Article domainModel, INavigationService navService, ITagRepository tagRepo, ICategoryRepository categoryRepo)
        {
            return new ArticleModel(navService, tagRepo, categoryRepo)
            {
                Id = domainModel.Id,
                PreviewPhotoPath = String.IsNullOrEmpty(domainModel.PreviewPhotoPath) ? "/img/development/dummy_1250x500.png" : domainModel.PreviewPhotoPath,
                ArticleHtmlPath = domainModel.FilePath ?? String.Empty,
                Tags = domainModel.Tags.Select(x => x.Name).ToList(),
                Title = domainModel.LongTitle ?? domainModel.ShortTitle,
                Published = domainModel.Created,
                LastUpdated = domainModel.LastUpdated,
                TextBeginning = domainModel.TextBeginning,
                GalleryExists = !String.IsNullOrEmpty(domainModel.GalleryUrl),
                PreviousArticleId = domainModel.PreviousArticle?.Id,
                PreviousArticleShortName = String.IsNullOrEmpty(domainModel.PreviousArticle?.LongTitle) ?
                                                domainModel.PreviousArticle?.ShortTitle :
                                                domainModel.PreviousArticle?.LongTitle,
                IsPreviousArticlePublished = domainModel.PreviousArticle?.IsPublished ?? false,
                NextArticleId = domainModel.NextArticle?.Id,
                NextArticleShortName = String.IsNullOrEmpty(domainModel.NextArticle?.LongTitle) ?
                                            domainModel.NextArticle?.ShortTitle :
                                            domainModel.NextArticle?.LongTitle,
                IsNextArticlePublished = domainModel.NextArticle?.IsPublished ?? false,
            };
        }

        public static ArticlePreviewModel MapArticlePreviewModel(Article domainModel, ICustomUrlProviderService customUrlProvider)
        {
            return new ArticlePreviewModel
            {
                Id = domainModel.Id,
                ShortTitle = domainModel.ShortTitle,
                IsPublished = domainModel.IsPublished,
                CustomUrl = customUrlProvider.HasCustomUrl("Article", "FullArticle", (key: "articleId", value: domainModel.Id.ToString())) ?
                                customUrlProvider.GetCustomUrl("Article", "FullArticle", (key: "articleId", value: domainModel.Id.ToString())) :
                                String.Empty
            };
        }

        public static AdminArticleModel MapAdminArticleModel(Article domainModel, ICustomUrlRepository customUrlRepository,
                                                             IArticleRepository articleRepository)
        {
            var result = new AdminArticleModel()
            {
                Id = domainModel.Id,
                IsPublished = domainModel.IsPublished,
                CategoryName = domainModel.Category?.Name ?? String.Empty,
                ShortTitle = domainModel.ShortTitle,
                LongTitle = domainModel.LongTitle,
                TextBeginning = domainModel.TextBeginning,
                Order = domainModel.Order,
                Tags = String.Join(", ", domainModel.Tags?.Select(x => x.Name) ?? Enumerable.Empty<string>()),
                FilePath = domainModel.FilePath,
                PreviewPhotoPath = domainModel.PreviewPhotoPath,
                Published = domainModel.Created.ToString("d. M. yyyy"),
                LastUpdated = domainModel.LastUpdated.ToString("d. M. yyyy"),
                CustomUrl = customUrlRepository.CustomUrls?.Where(x => x.OriginalUrl == $"/Article/FullArticle?articleId={domainModel.Id}").FirstOrDefault()?.NewUrl ?? String.Empty,
                GalleryUrl = domainModel.GalleryUrl,
                NextArticleId = domainModel.NextArticle?.Id,
                PreviousArticleId = domainModel.PreviousArticle?.Id,
            };
            foreach (var article in articleRepository.Articles.Where(x => x.Id != domainModel.Id))
            {
                var dropdownModel = new ArticleDropdownModel()
                {
                    Id = article.Id,
                    Title = String.IsNullOrEmpty(article.LongTitle) ? article.ShortTitle : article.LongTitle
                };
                result.AllArticles.Add(dropdownModel);
            }
            result.AllArticles = result.AllArticles.OrderBy(x => x.Title).ToList();

            return result;
        }
        #endregion Admin
    }
}
