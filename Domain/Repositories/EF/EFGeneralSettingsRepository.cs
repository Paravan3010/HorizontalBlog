using Horizontal.Domain.Contexts;

namespace Horizontal.Domain.Repositories.EF
{
    public class EFGeneralSettingsRepository : IGeneralSettingsRepository
    {
        private HorizontalDbContext _context;

        public EFGeneralSettingsRepository(HorizontalDbContext context)
        {
            _context = context;
        }
        public IQueryable<GeneralSettings> GeneralSettings => _context.GeneralSettings;

        public void SaveGeneralSettings(GeneralSettings generalSettings)
        {
            if (!GeneralSettings.Contains(generalSettings))
                _context.Add(generalSettings);

            _context.SaveChanges();
        }
    }
}
