using E_Commerce_Interno.Domain.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace E_Commerce_Inern_Project.Core.Features.Products
{
    /// <summary>
    /// Shared by the create, update and delete product handlers, so it stays in one place
    /// instead of being duplicated across them.
    /// </summary>
    public class ProductImageStorage
    {
        private readonly ILogger<ProductImageStorage> _logger;

        public ProductImageStorage(IWebHostEnvironment env, ILogger<ProductImageStorage> logger)
        {
            // WebRootPath is null until wwwroot exists on disk, which is the case on a fresh container.
            CurrentDir = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"), "ProductsImages");
            _logger = logger;
        }

        public string CurrentDir { get; }

        public string CategoryDir(Category Category)
            => Path.Combine(CurrentDir, Category.ParentCategory.CategoryName, Category.CategoryName);

        public async Task<string?> UploadImage(IFormFile image, Category Category)
        {
            string ImageDir = CategoryDir(Category);
            if (!Directory.Exists(ImageDir))
            {
                Directory.CreateDirectory(ImageDir);
            }

            string imagename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            try
            {
                using (FileStream stream = new(Path.Combine(ImageDir, imagename), FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return imagename;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed To Upload Image");

                return null;
            }
        }

        public async Task<string?> UpdateImage(IFormFile image, string oldImage, Category Category)
        {
            string ImageDir = Path.Combine(CategoryDir(Category), oldImage);
            if (!DeleteImage(ImageDir))
            {
                return null;
            }
            return await UploadImage(image, Category);
        }

        public bool DeleteImage(string ImagePath)
        {
            if (!File.Exists(ImagePath))
            {
                _logger.LogError("Image Doesnt Exists");
                return false;
            }
            try
            {
                File.Delete(ImagePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed To Delete Old Image");

                return false;
            }
        }

        public string HashImage(IFormFile image)
        {
            using (var sha256 = SHA256.Create())
            using (Stream stream = image.OpenReadStream())
            {
                byte[] hashBytes = sha256.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
