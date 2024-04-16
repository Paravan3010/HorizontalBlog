using Horizontal.Models.Navigation;

namespace Horizontal.Services
{
    public interface INavigationService
    {
        /// <summary>
        /// Returns (pregenerated) category navigation.
        /// </summary>
        /// <returns>Category navigation</returns>
        public IList<CategoryNavigationModel> GetCategoryNavigation();

        /// <summary>
        /// This method updates the category navigation.
        /// This method should be called whenever an article or cateogry is added, renamed, or deleted.
        /// </summary>
        public void ActualizeCategoryNavigation();
    }
}
