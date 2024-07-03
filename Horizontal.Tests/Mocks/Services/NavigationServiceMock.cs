using Horizontal.Models.Navigation;
using Horizontal.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizontal.Tests.Mocks.Services
{
    internal static class NavigationServiceMock
    {
        internal static Mock<INavigationService> GetMock()
        {

            var mock = new Mock<INavigationService>();
            mock.Setup(m => m.GetCategoryNavigation())
                   .Returns(new List<CategoryNavigationModel>()
                   {
                       new CategoryNavigationModel()
                       {
                           CategoryId = 1,
                           Name = "Category 1",
                           Articles = new List<ArticleNavigationModel>()
                           {
                               new ArticleNavigationModel()
                               {
                                   ArticleId = 1,
                                   Name = "Article 1"
                               },
                               new ArticleNavigationModel()
                               {
                                   ArticleId = 2,
                                   Name = "Article 2"
                               },
                               new ArticleNavigationModel()
                               {
                                   ArticleId = 3,
                                   Name = "Article 3"
                               }
                           },
                           Subcategorie = new List<CategoryNavigationModel>()
                           {
                               new CategoryNavigationModel()
                               {
                                   CategoryId = 2,
                                   Name = "Category 1 - 1",
                                   Articles = new List<ArticleNavigationModel>()
                                   {
                                       new ArticleNavigationModel
                                       {
                                           ArticleId = 4,
                                           Name = "Article 4"
                                       },
                                       new ArticleNavigationModel
                                       {
                                           ArticleId = 5,
                                           Name = "Article 5"
                                       }
                                   }
                               },
                               new CategoryNavigationModel()
                               {
                                   CategoryId = 3,
                                   Name = "Category 1 - 2",
                                   Articles = new List<ArticleNavigationModel>()
                                   {
                                       new ArticleNavigationModel
                                       {
                                           ArticleId = 6,
                                           Name = "Article 6"
                                       },
                                       new ArticleNavigationModel
                                       {
                                           ArticleId = 7,
                                           Name = "Article 7"
                                       }
                                   }
                               }
                           }
                       },
                       new CategoryNavigationModel()
                       {
                           CategoryId = 4,
                           Name = "Category 2",
                           Subcategorie = new List<CategoryNavigationModel>()
                           {
                               new CategoryNavigationModel()
                               {
                                   CategoryId = 5,
                                   Name = "Category 2 - 1"
                               },
                               new CategoryNavigationModel()
                               {
                                   CategoryId = 6,
                                   Name = "Category 2 - 2",
                                   Subcategorie = new List<CategoryNavigationModel>()
                                   {
                                       new CategoryNavigationModel()
                                       {
                                           CategoryId = 7,
                                           Name = "Category 2 - 2 - 1",
                                           Articles = new List<ArticleNavigationModel>()
                                           {
                                                new ArticleNavigationModel
                                                {
                                                    ArticleId = 8,
                                                    Name = "Article 8"
                                                }
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   });

            return mock;
        }
    }
}
