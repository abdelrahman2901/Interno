
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;
        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> IsCategoryExistsByName(string CategoryName)
        {
            try
            {
                return await _context.Category.AnyAsync(s => s.CategoryName == CategoryName &&!s.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured At IsCategoryExistsByName Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured At IsCategoryExistsByName InnerException: {message}", ex.InnerException.Message);
                } 
                return false;
            }
        }

        public async Task<bool> IsCategoryExistsByID(Guid? CategoryID)
        {
            try
            {
                return await _context.Category.AnyAsync(s => s.CategoryID == CategoryID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While Checking If cat exists by id Exception: {message}",ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While X InnerException: {message}", ex.InnerException.Message);
                }

                return false;
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {

                return await _context.Category.AsNoTracking().Where(p=>p.ParentCategoryID==null && !p.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured At IsCategoryExistsByIDException: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured At IsCategoryExistsByID InnerException: {message}", ex.InnerException.Message);
                }
                return [];
            }
        }
        public async Task<IEnumerable<Category?>> GetCategoriesWithSubCat()
        {
            try
            {
                return await _context.Category.AsNoTracking().Where(p=>p.ParentCategoryID==null && !p.IsDeleted).Include(p => p.SubCategories.Where(p=>!p.IsDeleted)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured At GetCategoriesWithSubCat Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured At GetCategoriesWithSubCat InnerException: {message}", ex.InnerException.Message);
                }
                return [];
            }

        }

        public async Task<Category?> GetCategoryById(Guid? CatID)
        {
            try
            {

                return await _context.Category.Include("ParentCategory").FirstOrDefaultAsync(c => c.CategoryID == CatID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured At GetCategoryById Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured At GetCategoryById InnerException: {message}", ex.InnerException.Message);
                }
                return null;
            }
        }



        public async Task<Category?> GetCategoryByIdIncludingSubCat(Guid? CatID)
        {
            try
            {

                return await _context.Category.Include("ParentCategory").Include("SubCategories").FirstOrDefaultAsync(c => c.CategoryID == CatID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured At GetCategoryById Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured At GetCategoryById InnerException: {message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<Category?> AddCategory(Category category)
        {
            try
            {
                await _context.Category.AddAsync(category);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While Adding NEw Category Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While Adding New Category  InnerException : {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        
        public async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While Saving Changing Category Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While Saving Changees Category InnerException : {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }

    }
}
