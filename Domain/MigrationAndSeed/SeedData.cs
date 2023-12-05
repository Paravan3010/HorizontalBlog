using Horizontal.Domain.Contexts;
using Horizontal.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Xml.Linq;

namespace Horizontal.Domain.MigrationAndSeed
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app, ConfigurationManager config)
        {
            var fillWithTestData = config["FillWithTestData"];
            if (!String.Equals("TRUE", fillWithTestData, StringComparison.OrdinalIgnoreCase))
                return;

            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<HorizontalDbContext>();

            // Do not perform if any table already contains data
            if (context.Categories.Any() || context.Articles.Any() || context.Tags.Any() || context.CustomUrls.Any())
                return;

            PopulateCategories(context);
            PopulateArticles(context);
            PopulateTags(context);
            PopulateCustomUrlMapping(context);
        }

        private static void PopulateCategories(HorizontalDbContext context)
        {
            var northAmerica = new Category() { Name = "North America", IsPublished = true };
            {
                var usa = new Category() { Name = "USA", ParentCategory = northAmerica, IsPublished = true };
                northAmerica.ChildCategories.Add(usa);
                {
                    var eastCoast = new Category() { Name = "East Coast", ParentCategory = usa, IsPublished = true };
                    usa.ChildCategories.Add(eastCoast);
                    var westCoast = new Category() { Name = "West Coast", ParentCategory = usa, IsPublished = true };
                    usa.ChildCategories.Add(westCoast);

                    context.Categories.AddRange(eastCoast, westCoast);
                }
                var canada = new Category() { Name = "Canada", ParentCategory = northAmerica, IsPublished = true };
                northAmerica.ChildCategories.Add(canada);

                context.Categories.AddRange(usa, canada);
            }
            var europe = new Category() { Name = "Europe", IsPublished = true };
            {
                var uk = new Category() { Name = "UK", ParentCategory = europe, IsPublished = false };
                europe.ChildCategories.Add(uk);
                var germany = new Category() { Name = "Germany", ParentCategory = europe, IsPublished = true };
                europe.ChildCategories.Add(germany);

                context.Categories.AddRange(uk, germany);
            }
            var asia = new Category() { Name = "Asia", IsPublished = true };
            {
                var mongolia = new Category() { Name = "Mongolia", ParentCategory = asia, IsPublished = false };
                asia.ChildCategories.Add(mongolia);
                var china = new Category() { Name = "China", ParentCategory = asia, IsPublished = true };
                asia.ChildCategories.Add(china);
                var india = new Category() { Name = "India", ParentCategory = asia, IsPublished = true };
                asia.ChildCategories.Add(india);

                context.Categories.AddRange(mongolia, china, india);
            }
            context.Categories.AddRange(northAmerica, europe, asia);

            context.SaveChanges();
        }

        private static void PopulateArticles(HorizontalDbContext context)
        {
            #region North American
            var eastCoastCategory = context.Categories.Where(x => x.Name == "East Coast").FirstOrDefault();
            var generalInformationEastCoast = new Article()
            {
                FilePath = "articles/general-info-east-coast.html",
                ShortTitle = "General Information",
                PreviewPhotoPath = "/img/general-info-east-coast/title.jpg",
                LongTitle = "General Information about traveling on the east coast",
                Category = eastCoastCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true,
                Order = 1,
            };
            eastCoastCategory?.Articles.Add(generalInformationEastCoast);
            var blueRidgeParkway = new Article()
            {
                FilePath = "articles/blue-ridge-parkway.html",
                ShortTitle = "Blue Ridge Parkway",
                PreviewPhotoPath = "/img/blue-ridge-parkway/title.jpg",
                LongTitle = "Ride on the Blue Ridge parkway through Virginia and North Carolina",
                Category = eastCoastCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true,
                Order = 2
            };
            eastCoastCategory?.Articles.Add(blueRidgeParkway);

            var usaCategory = context.Categories.Where(x => x.Name == "USA").FirstOrDefault();
            var usaVisa = new Article()
            {
                FilePath = "articles/usa-visa.html",
                ShortTitle = "Visa requirements",
                PreviewPhotoPath = "/img/usa-visa/title.jpg",
                LongTitle = "USA visa requirements and rules",
                Category = usaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            usaCategory?.Articles.Add(usaVisa);

            var canadaCategory = context.Categories.Where(x => x.Name == "Canada").FirstOrDefault();
            var niagaraFalls = new Article()
            {
                FilePath = "articles/niagara-falls.html",
                ShortTitle = "Niagara Falls",
                PreviewPhotoPath = "/img/niagara-falls/title.jpg",
                LongTitle = "My visit to Niagara Falls",
                Category = canadaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            canadaCategory?.Articles.Add(niagaraFalls);
            #endregion North America

            #region Europe
            var europeCategory = context.Categories.Where(x => x.Name == "Europe").FirstOrDefault();
            var schengen = new Article()
            {
                FilePath = "articles/schengen.html",
                ShortTitle = "Schengen Area",
                PreviewPhotoPath = "/img/schengen/title.jpg",
                LongTitle = "Schengen area overview",
                Category = europeCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            europeCategory?.Articles.Add(schengen);

            var ukCategory = context.Categories.Where(x => x.Name == "UK").FirstOrDefault();
            var ukVisa = new Article()
            {
                FilePath = "articles/uk-visa.html",
                ShortTitle = "Visa",
                PreviewPhotoPath = "/img/uk-visa/title.jpg",
                LongTitle = "Visa to the UK",
                Category = ukCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = false
            };
            ukCategory?.Articles.Add(ukVisa);

            var germanyCategory = context.Categories.Where(x => x.Name == "Germany").FirstOrDefault();
            var castlesGermany = new Article()
            {
                FilePath = "articles/castles-in-germany.html",
                ShortTitle = "Castles of Germany",
                PreviewPhotoPath = "/img/castles-in-germany/title.jpg",
                LongTitle = "The best castles and fortresses to visit in Gemany",
                Category = germanyCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true,
                Order = 1
            };
            germanyCategory?.Articles.Add(castlesGermany);
            var germainRailway = new Article()
            {
                FilePath = "articles/german-railway.html",
                ShortTitle = "Deutsche Bahn Guide",
                PreviewPhotoPath = "/img/german-railway/title.jpg",
                LongTitle = "Guide to using trains in Germany",
                Category = germanyCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true,
                Order = 2
            };
            germanyCategory?.Articles.Add(germainRailway);
            #endregion Europe

            #region Asia
            var asiaCategory = context.Categories.Where(x => x.Name == "Asia").FirstOrDefault();
            var asiaVisaGuide = new Article()
            {
                FilePath = "articles/asian-visa-guide.html",
                ShortTitle = "Visa guide",
                PreviewPhotoPath = "/img/asian-visa-guide/title.jpg",
                LongTitle = "Guide to visas in Asia",
                Category = asiaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            asiaCategory?.Articles.Add(asiaVisaGuide);
            var vietnam = new Article()
            {
                FilePath = "articles/vietnam.html",
                ShortTitle = "Vietnam travels",
                PreviewPhotoPath = "/img/vietnam/title.jpg",
                LongTitle = "My travels through Vietnam: South to North by train, moped and bike",
                Category = asiaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            asiaCategory?.Articles.Add(vietnam);

            var mongoliaCategory = context.Categories.Where(x => x.Name == "Mongolia").FirstOrDefault();
            var mongoliaByCar = new Article()
            {
                FilePath = "articles/mongolia.html",
                ShortTitle = "Mongolia by car",
                PreviewPhotoPath = "/img/mongolia/title.jpg",
                LongTitle = "What not to miss when traveling through Mongolia by car",
                Category = mongoliaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            mongoliaCategory?.Articles.Add(mongoliaByCar);
            var mongoliaCarRentalGuide = new Article()
            {
                FilePath = "articles/mongolia-car-rental.html",
                ShortTitle = "Car rental guide",
                PreviewPhotoPath = "/img/mongolia-car-rental/title.jpg",
                LongTitle = "Mongolia: car rental guide",
                Category = mongoliaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            mongoliaCategory?.Articles.Add(mongoliaCarRentalGuide);

            var chinaCategory = context.Categories.Where(x => x.Name == "China").FirstOrDefault();
            var chineseWall = new Article()
            {
                FilePath = "articles/chinese-wall.html",
                ShortTitle = "Chinese wall from Beijing",
                PreviewPhotoPath = "/img/chinese-wall/title.jpg",
                LongTitle = "How I visited the Great Wall of China from Beijing",
                Category = chinaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            chinaCategory?.Articles.Add(chineseWall);
            var chineseRailway = new Article()
            {
                FilePath = "articles/chinese-railway.html",
                ShortTitle = "Guide to train travel",
                PreviewPhotoPath = "/img/chinese-railway/title.jpg",
                LongTitle = "How to use trains in China",
                Category = chinaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            chinaCategory?.Articles.Add(chineseRailway);

            var indiaCategory = context.Categories.Where(x => x.Name == "India").FirstOrDefault();
            var indianRailway = new Article()
            {
                FilePath = "articles/indian-railway.html",
                ShortTitle = "Guide to train travel",
                PreviewPhotoPath = "/img/indian-railway/title.jpg",
                LongTitle = "How to use trains in India",
                Category = indiaCategory,
                Created = DateTime.Now,
                TextBeginning = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. In convallis. Nulla est. Mauris dictum facilisis augue. Aliquam ante. Aenean placerat. In rutrum. Nullam sit amet magna in magna gravida vehicula. Nulla quis diam. Maecenas aliquet accumsan leo. Proin in tellus sit amet nibh dignissim sagittis. Nullam eget nisl. Nulla non arcu lacinia neque faucibus fringilla. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nullam dapibus fermentum ipsum. Fusce consectetuer risus a nunc.",
                IsPublished = true
            };
            indiaCategory?.Articles.Add(indianRailway);
            #endregion Asia

            context.Articles.AddRange(generalInformationEastCoast, blueRidgeParkway, usaVisa, niagaraFalls,
                                      schengen, ukVisa, castlesGermany, germainRailway, asiaVisaGuide,
                                      vietnam, mongoliaByCar, mongoliaCarRentalGuide, chineseWall, chineseRailway,
                                      indianRailway);

            context.SaveChanges();

            // follow-up articles need to be saved separately to prevent circular dependency error
            chineseWall.NextArticle = chineseRailway;
            chineseRailway.NextArticle = indianRailway;
            context.SaveChanges();

            chineseRailway.PreviousArticle = chineseWall;
            indianRailway.PreviousArticle = chineseRailway;
            context.SaveChanges();
        }

        private static void PopulateTags(HorizontalDbContext context)
        {
            #region Create tags
            var nature = new Tag()
            {
                Name = "Nature",
                IsPublished = true,
                IsInTopNavbar = true,
                TopNavbarOrder = 1
            };
            var culture = new Tag()
            {
                Name = "Culture",
                IsPublished = true,
                IsInTopNavbar = true,
                TopNavbarOrder = 2
            };
            var roads = new Tag()
            {
                Name = "Roads",
                IsPublished = true,
                IsInTopNavbar = false
            };
            var generalInfo = new Tag()
            {
                Name = "General Info",
                IsPublished = true,
                IsInTopNavbar = true,
                TopNavbarOrder = 3
            };
            var travel = new Tag()
            {
                Name = "Travel",
                IsPublished = false,
                IsInTopNavbar = false
            };
            var visa = new Tag()
            {
                Name = "Visa",
                IsPublished = true,
                IsInTopNavbar = true,
                TopNavbarOrder = 4
            };
            var history = new Tag()
            {
                Name = "History",
                IsPublished = true,
                IsInTopNavbar = true,
                TopNavbarOrder = 5
            };
            var trains = new Tag()
            {
                Name = "Trains",
                IsPublished = true,
                IsInTopNavbar = true,
                TopNavbarOrder = 6
            };
            var virginia = new Tag()
            {
                Name = "Virginia",
                IsPublished = true,
                IsInTopNavbar = false
            };
            var northCarolina = new Tag()
            {
                Name = "North Carolina",
                IsPublished = true,
                IsInTopNavbar = false
            };
            #endregion Create tags

            #region ADD ARTICLES TO TAGS: North America
            var generalInformationEastCoast = context.Articles.Where(x => x.ShortTitle == "General Information" && x.Category != null && x.Category.Name == "East Coast").First();
            generalInformationEastCoast.Tags.Add(generalInfo);
            generalInformationEastCoast.Tags.Add(travel);
            generalInfo.Articles.Add(generalInformationEastCoast);
            travel.Articles.Add(generalInformationEastCoast);

            var blueRidgeParkway = context.Articles.Where(x => x.ShortTitle == "Blue Ridge Parkway" && x.Category != null && x.Category.Name == "East Coast").First();
            blueRidgeParkway.Tags.Add(travel);
            blueRidgeParkway.Tags.Add(roads);
            blueRidgeParkway.Tags.Add(nature);
            blueRidgeParkway.Tags.Add(virginia);
            blueRidgeParkway.Tags.Add(northCarolina);
            travel.Articles.Add(blueRidgeParkway);
            roads.Articles.Add(blueRidgeParkway);
            nature.Articles.Add(blueRidgeParkway);
            virginia.Articles.Add(blueRidgeParkway);
            northCarolina.Articles.Add(blueRidgeParkway);

            var usaVisa = context.Articles.Where(x => x.ShortTitle == "Visa requirements" && x.Category != null && x.Category.Name == "USA").First();
            usaVisa.Tags.Add(generalInfo);
            usaVisa.Tags.Add(travel);
            usaVisa.Tags.Add(visa);
            generalInfo.Articles.Add(usaVisa);
            travel.Articles.Add(usaVisa);
            visa.Articles.Add(usaVisa);

            var niagaraFalls = context.Articles.Where(x => x.ShortTitle == "Niagara Falls" && x.Category != null && x.Category.Name == "Canada").First();
            niagaraFalls.Tags.Add(nature);
            niagaraFalls.Tags.Add(travel);
            nature.Articles.Add(niagaraFalls);
            travel.Articles.Add(niagaraFalls);
            #endregion ADD ARTICLES TO TAGS: North America

            #region ADD ARTICLES TO TAGS: Europe
            var schengen = context.Articles.Where(x => x.ShortTitle == "Schengen Area" && x.Category != null && x.Category.Name == "Europe").First();
            schengen.Tags.Add(travel);
            schengen.Tags.Add(generalInfo);
            schengen.Tags.Add(visa);
            travel.Articles.Add(schengen);
            generalInfo.Articles.Add(schengen);
            visa.Articles.Add(schengen);

            var ukVisa = context.Articles.Where(x => x.ShortTitle == "Visa" && x.Category != null && x.Category.Name == "UK").First();
            ukVisa.Tags.Add(travel);
            ukVisa.Tags.Add(generalInfo);
            ukVisa.Tags.Add(visa);
            travel.Articles.Add(ukVisa);
            generalInfo.Articles.Add(ukVisa);
            visa.Articles.Add(ukVisa);

            var castlesGermany = context.Articles.Where(x => x.ShortTitle == "Castles of Germany" && x.Category != null && x.Category.Name == "Germany").First();
            castlesGermany.Tags.Add(travel);
            castlesGermany.Tags.Add(history);
            castlesGermany.Tags.Add(culture);
            travel.Articles.Add(castlesGermany);
            history.Articles.Add(castlesGermany);
            culture.Articles.Add(castlesGermany);

            var germainRailway = context.Articles.Where(x => x.ShortTitle == "Deutsche Bahn Guide" && x.Category != null && x.Category.Name == "Germany").First();
            germainRailway.Tags.Add(travel);
            germainRailway.Tags.Add(generalInfo);
            germainRailway.Tags.Add(trains);
            travel.Articles.Add(germainRailway);
            generalInfo.Articles.Add(germainRailway);
            trains.Articles.Add(germainRailway);
            #endregion ADD ARTICLES TO TAGS: Europe

            #region ADD ARTICLES TO TAGS: Asia
            var asiaVisaGuide = context.Articles.Where(x => x.ShortTitle == "Visa guide" && x.Category != null && x.Category.Name == "Asia").First();
            asiaVisaGuide.Tags.Add(travel);
            asiaVisaGuide.Tags.Add(generalInfo);
            asiaVisaGuide.Tags.Add(visa);
            travel.Articles.Add(asiaVisaGuide);
            generalInfo.Articles.Add(asiaVisaGuide);
            visa.Articles.Add(asiaVisaGuide);

            var vietnam = context.Articles.Where(x => x.ShortTitle == "Vietnam travels" && x.Category != null && x.Category.Name == "Asia").First();
            vietnam.Tags.Add(travel);
            vietnam.Tags.Add(nature);
            vietnam.Tags.Add(history);
            vietnam.Tags.Add(culture);
            travel.Articles.Add(vietnam);
            nature.Articles.Add(vietnam);
            history.Articles.Add(vietnam);
            culture.Articles.Add(vietnam);

            var mongoliaByCar = context.Articles.Where(x => x.ShortTitle == "Mongolia by car" && x.Category != null && x.Category.Name == "Mongolia").First();
            mongoliaByCar.Tags.Add(travel);
            mongoliaByCar.Tags.Add(roads);
            mongoliaByCar.Tags.Add(nature);
            mongoliaByCar.Tags.Add(culture);
            mongoliaByCar.Tags.Add(history);
            travel.Articles.Add(mongoliaByCar);
            roads.Articles.Add(mongoliaByCar);
            nature.Articles.Add(mongoliaByCar);
            culture.Articles.Add(mongoliaByCar);
            history.Articles.Add(mongoliaByCar);

            var mongoliaCarRentalGuide = context.Articles.Where(x => x.ShortTitle == "Car rental guide" && x.Category != null && x.Category.Name == "Mongolia").First();
            mongoliaCarRentalGuide.Tags.Add(travel);
            mongoliaCarRentalGuide.Tags.Add(roads);
            mongoliaCarRentalGuide.Tags.Add(generalInfo);
            travel.Articles.Add(mongoliaCarRentalGuide);
            roads.Articles.Add(mongoliaCarRentalGuide);
            generalInfo.Articles.Add(mongoliaCarRentalGuide);

            var chineseWall = context.Articles.Where(x => x.ShortTitle == "Chinese wall from Beijing" && x.Category != null && x.Category.Name == "China").First();
            chineseWall.Tags.Add(travel);
            chineseWall.Tags.Add(history);
            chineseWall.Tags.Add(culture);
            chineseWall.Tags.Add(nature);
            chineseWall.Tags.Add(generalInfo);
            travel.Articles.Add(chineseWall);
            history.Articles.Add(chineseWall);
            culture.Articles.Add(chineseWall);
            nature.Articles.Add(chineseWall);
            generalInfo.Articles.Add(chineseWall);

            var trainTravelChina = context.Articles.Where(x => x.ShortTitle == "Guide to train travel" && x.Category != null && x.Category.Name == "China").First();
            trainTravelChina.Tags.Add(travel);
            trainTravelChina.Tags.Add(generalInfo);
            trainTravelChina.Tags.Add(trains);
            trainTravelChina.Tags.Add(visa);
            travel.Articles.Add(trainTravelChina);
            generalInfo.Articles.Add(trainTravelChina);
            trains.Articles.Add(trainTravelChina);
            visa.Articles.Add(trainTravelChina);

            var trainTravelIndia = context.Articles.Where(x => x.ShortTitle == "Guide to train travel" && x.Category != null && x.Category.Name == "India").First();
            trainTravelIndia.Tags.Add(travel);
            trainTravelIndia.Tags.Add(generalInfo);
            trainTravelIndia.Tags.Add(trains);
            trainTravelIndia.Tags.Add(visa);
            travel.Articles.Add(trainTravelIndia);
            generalInfo.Articles.Add(trainTravelIndia);
            trains.Articles.Add(trainTravelIndia);
            visa.Articles.Add(trainTravelIndia);
            #endregion ADD ARTICLES TO TAGS: Asia

            context.SaveChanges();
        }

        private static void PopulateCustomUrlMapping(HorizontalDbContext context)
        {
            #region Tags
            var natureUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=Nature", NewUrl = "/tag-nature" };
            var cultureUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=Culture", NewUrl = "/tag-culture" };
            var roadsUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=Roads", NewUrl = "/tag-roads" };
            var generalInfoUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=General%20Info", NewUrl = "/tag-general-info" };
            var travelUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=Travel", NewUrl = "/tag-travel" };
            var visaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=Visa", NewUrl = "/tag-visa" };
            var historyUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=History", NewUrl = "/tag-history" };
            var trainsUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=Trains", NewUrl = "/tag-trains" };
            var virginiaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=Virginia", NewUrl = "/tag-virginia" };
            var northCarolinaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = "/Category/Tag?tagName=North%20Carolina", NewUrl = "/tag-north-carolina" };

            context.CustomUrls.AddRange(natureUrlMapping, cultureUrlMapping, roadsUrlMapping, generalInfoUrlMapping, travelUrlMapping,
                                        visaUrlMapping, historyUrlMapping, trainsUrlMapping, virginiaUrlMapping, northCarolinaUrlMapping);
            #endregion Tags

            #region Categories
            var northAmericaCategory = context.Categories.Where(x => x.Name == "North America").FirstOrDefault();
            var northAmericaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={northAmericaCategory.Id}", NewUrl = "/north-america" };

            var usaCategory = context.Categories.Where(x => x.Name == "USA").FirstOrDefault();
            var usaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={usaCategory.Id}", NewUrl = "/usa" };

            var eastCoastCategory = context.Categories.Where(x => x.Name == "East Coast").FirstOrDefault();
            var eastCoastUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={eastCoastCategory.Id}", NewUrl = "/east-coast" };

            var westCoastCategory = context.Categories.Where(x => x.Name == "West Coast").FirstOrDefault();
            var westCoastUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={westCoastCategory.Id}", NewUrl = "/west-coast" };

            var canadaCategory = context.Categories.Where(x => x.Name == "Canada").FirstOrDefault();
            var canadaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={canadaCategory.Id}", NewUrl = "/canada" };

            var europeCategory = context.Categories.Where(x => x.Name == "Europe").FirstOrDefault();
            var europeUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={europeCategory.Id}", NewUrl = "/europe" };

            var ukCategory = context.Categories.Where(x => x.Name == "UK").FirstOrDefault();
            var ukUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={ukCategory.Id}", NewUrl = "/uk" };

            var germanyCategory = context.Categories.Where(x => x.Name == "Germany").FirstOrDefault();
            var germanyUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={germanyCategory.Id}", NewUrl = "/germany" };

            var asiaCategory = context.Categories.Where(x => x.Name == "Asia").FirstOrDefault();
            var asiaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={asiaCategory.Id}", NewUrl = "/asia" };

            var mongoliaCategory = context.Categories.Where(x => x.Name == "Mongolia").FirstOrDefault();
            var mongoliaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={mongoliaCategory.Id}", NewUrl = "/mongolia" };

            var chinaCategory = context.Categories.Where(x => x.Name == "China").FirstOrDefault();
            var chinaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={chinaCategory.Id}", NewUrl = "/china" };

            var indiaCategory = context.Categories.Where(x => x.Name == "India").FirstOrDefault();
            var indiaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Category/Category?categoryId={indiaCategory.Id}", NewUrl = "/india" };

            context.CustomUrls.AddRange(northAmericaUrlMapping, usaUrlMapping, eastCoastUrlMapping, westCoastUrlMapping, canadaUrlMapping, europeUrlMapping,
                                        ukUrlMapping, germanyUrlMapping, asiaUrlMapping, mongoliaUrlMapping, chinaUrlMapping, chinaUrlMapping);
            #endregion Categories

            #region Articles
            var generalInformationEastCoast = context.Articles.Where(x => x.ShortTitle == "General Information" && x.Category != null && x.Category.Name == "East Coast").First();
            var generalInformationEastCoastUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={generalInformationEastCoast.Id}", NewUrl = "/east-coast-general-info" };

            var blueRidgeParkway = context.Articles.Where(x => x.ShortTitle == "Blue Ridge Parkway" && x.Category != null && x.Category.Name == "East Coast").First();
            var blueRidgeParkwayUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={blueRidgeParkway.Id}", NewUrl = "/blue-ridge-parkway" };

            var usaVisa = context.Articles.Where(x => x.ShortTitle == "Visa requirements" && x.Category != null && x.Category.Name == "USA").First();
            var usaVisaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={usaVisa.Id}", NewUrl = "/usa-visa" };

            var niagaraFalls = context.Articles.Where(x => x.ShortTitle == "Niagara Falls" && x.Category != null && x.Category.Name == "Canada").First();
            var niagaraFallsUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={niagaraFalls.Id}", NewUrl = "/niagara-falls" };

            var schengen = context.Articles.Where(x => x.ShortTitle == "Schengen Area" && x.Category != null && x.Category.Name == "Europe").First();
            var schengenUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={schengen.Id}", NewUrl = "/schengen" };

            var ukVisa = context.Articles.Where(x => x.ShortTitle == "Visa" && x.Category != null && x.Category.Name == "UK").First();
            var ukVisaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={ukVisa.Id}", NewUrl = "/uk-visa" };

            var castlesGermany = context.Articles.Where(x => x.ShortTitle == "Castles of Germany" && x.Category != null && x.Category.Name == "Germany").First();
            var castlesGermanyUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={castlesGermany.Id}", NewUrl = "/castles-of-germany" };

            var germainRailway = context.Articles.Where(x => x.ShortTitle == "Deutsche Bahn Guide" && x.Category != null && x.Category.Name == "Germany").First();
            var germainRailwayUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={germainRailway.Id}", NewUrl = "/db-german-train-guide" };

            var asiaVisaGuide = context.Articles.Where(x => x.ShortTitle == "Visa guide" && x.Category != null && x.Category.Name == "Asia").First();
            var asiaVisaGuideUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={asiaVisaGuide.Id}", NewUrl = "/asia-visa" };

            var vietnam = context.Articles.Where(x => x.ShortTitle == "Vietnam travels" && x.Category != null && x.Category.Name == "Asia").First();
            var vietnamUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={vietnam.Id}", NewUrl = "/vietnam-travels" };

            var mongoliaByCar = context.Articles.Where(x => x.ShortTitle == "Mongolia by car" && x.Category != null && x.Category.Name == "Mongolia").First();
            var mongoliaByCarUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={mongoliaByCar.Id}", NewUrl = "/mongolia-by-car" };

            var mongoliaCarRentalGuide = context.Articles.Where(x => x.ShortTitle == "Car rental guide" && x.Category != null && x.Category.Name == "Mongolia").First();
            var mongoliaCarRentalGuideUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={mongoliaCarRentalGuide.Id}", NewUrl = "/mongolia-care-rental-guide" };

            var chineseWall = context.Articles.Where(x => x.ShortTitle == "Chinese wall from Beijing" && x.Category != null && x.Category.Name == "China").First();
            var chineseWallUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={chineseWall.Id}", NewUrl = "/chinese-wall" };

            var trainTravelChina = context.Articles.Where(x => x.ShortTitle == "Guide to train travel" && x.Category != null && x.Category.Name == "China").First();
            var trainTravelChinaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={trainTravelChina.Id}", NewUrl = "/china-train-travel-guide" };

            var trainTravelIndia = context.Articles.Where(x => x.ShortTitle == "Guide to train travel" && x.Category != null && x.Category.Name == "India").First();
            var trainTravelIndiaUrlMapping = new CustomUrl() { IsActive = true, OriginalUrl = $"/Article/FullArticle?articleId={trainTravelIndia.Id}", NewUrl = "/india-train-travel-guide" };

            context.CustomUrls.AddRange(generalInformationEastCoastUrlMapping, blueRidgeParkwayUrlMapping, usaVisaUrlMapping, niagaraFallsUrlMapping, schengenUrlMapping,
                                        ukVisaUrlMapping, castlesGermanyUrlMapping, germainRailwayUrlMapping, asiaVisaGuideUrlMapping, vietnamUrlMapping,
                                        mongoliaByCarUrlMapping, mongoliaCarRentalGuideUrlMapping, chineseWallUrlMapping, trainTravelChinaUrlMapping,
                                        trainTravelIndiaUrlMapping);
            #endregion Articles

            context.SaveChanges();
        }
    }
}