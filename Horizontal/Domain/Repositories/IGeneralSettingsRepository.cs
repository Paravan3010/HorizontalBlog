namespace Horizontal.Domain.Repositories
{
    public interface IGeneralSettingsRepository
    {
        IQueryable<GeneralSettings> GeneralSettings { get; }

        void SaveGeneralSettings(GeneralSettings generalSettings);
    }
}
