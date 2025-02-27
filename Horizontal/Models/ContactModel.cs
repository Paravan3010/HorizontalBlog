using Horizontal.Domain.Repositories;
using Horizontal.Models;
using Horizontal.Services;

namespace Horizontal.Models
{
    public class ContactModel : BaseModel
    {
        public ContactModel(INavigationService navigationService, 
                            ITagRepository tagRepository, 
                            ICategoryRepository categoryRepository) : base(navigationService, tagRepository, categoryRepository)
        {
        }
    }
}
