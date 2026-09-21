using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICategoryRepo
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllCategories();
        public Task<Category?> GetCategoryById(Guid? CategoryID);
        public Task<IEnumerable<Category>> GetCategoriesWithSubCat();
        public Task<Category?> GetCategoryByIdIncludingSubCat(Guid? CatID);

        public Task<Category?> AddCategory(Category category);
        //public  Category? UpdateCategory(Category category);
        //public bool DeleteCategory(Category category);
        public Task<bool> SaveChanges();
        public Task<bool> IsCategoryExistsByName(string CategoryName);
        public Task<bool> IsCategoryExistsByID(Guid? CategoryID);

    }
}
