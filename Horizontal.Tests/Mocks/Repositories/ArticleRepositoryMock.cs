using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizontal.Tests.Mocks.Repositories
{
    internal class ArticleRepositoryMock
    {
        internal static Mock<IArticleRepository> GetMock()
        {
            var mock = new Mock<IArticleRepository>();
            mock.Setup(m => m.Articles)
                .Returns((new Article[]
                {
                    new Article
                    {
                        Id = 1,
                        ShortTitle = "Article 1",
                        LongTitle = "Long title article one",
                        Category = new Category()
                        {
                            Id = 1,
                            Name = "Category 1",
                            IsPublished = true
                        },
                        Created = new DateTime(2021, )
                    }
                }).AsQueryable<Article>());
        }
    }
}
