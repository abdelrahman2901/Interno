
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.ICategoryRepo
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllCategories();
        public Task<IEnumerable<Category>> GetCategoriesWithSubCat();
        public Task<Category?> GetCategoryById(Guid? CategoryID);
        public Task<Category?> GetCategoryByIdIncludingSubCat(Guid? CatID);
        public Task<bool> AddCategory(Category category);
        public Task<bool> SaveChanges();
        public Task<bool> IsCategoryExistsByName(string CategoryName);
        public Task<bool> IsCategoryExistsByID(Guid? CategoryID);

    }
}
