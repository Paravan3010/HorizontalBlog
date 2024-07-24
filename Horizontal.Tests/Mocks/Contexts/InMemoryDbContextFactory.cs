using Horizontal.Domain.Contexts;
using Horizontal.Domain.MigrationAndSeed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizontal.Tests.Mocks.Contexts
{
    public static class InMemoryDbContextFactory
    {
        public static HorizontalDbContext CreateHorizontalDbContext()
        {
            var options = new DbContextOptionsBuilder<HorizontalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Each context instance returned has its separate database
                .Options;

            var context = new HorizontalDbContext(options);

            SeedData.Populate(context);

            return context;
        }
    }
}
