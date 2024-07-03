using Horizontal.Controllers;
using Horizontal.Models.Navigation;
using Horizontal.Services;
using Horizontal.Tests.Mocks.Services;
using Moq;

namespace Horizontal.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CanLoadMainPage()
        {
            // Arrange

        }

        private void GetMockedController()
        {
            // Article Repository

            // Tag Repository

            // Category Repository

            // General Settings Repository

            // Article Tag Repository

            return new HomeController(NavigationServiceMock.GetMock(), );
        }
    }
}