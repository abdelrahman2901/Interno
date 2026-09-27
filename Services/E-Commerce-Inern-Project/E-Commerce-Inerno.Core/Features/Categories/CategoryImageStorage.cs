using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace E_Commerce_Inern_Project.Core.Features.Categories
{
    /// <summary>
    /// Shared by the create, update and delete category handlers, so it stays in one place
    /// instead of being duplicated across them.
    /// </summary>
    public class CategoryImageStorage
    {
        private readonly ILogger<CategoryImageStorage> _logger;

        public CategoryImageStorage(IWebHostEnvironment env, ILogger<CategoryImageStorage> logger)
        {
            // WebRootPath is null until wwwroot exists on disk, which is the case on a fresh container.
            CurrentDir = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"), "CategoryImages");
            _logger = logger;
        }

        public string CurrentDir { get; }

        public async Task<string?> UploadImage(IFormFile image)
        {
            if (!Directory.Exists(CurrentDir))
            {
                Directory.CreateDirectory(CurrentDir);
            }

            string imagename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            try
            {
                using (FileStream stream = new(Path.Combine(CurrentDir, imagename), FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return imagename;
            }
            catch (Exception)
            {
                _logger.LogError("Failed To Upload Image");

                return null;
            }
        }

        public async Task<string?> UpdateImage(IFormFile image, string oldImage)
        {
            string ImageDir = Path.Combine(CurrentDir, oldImage);
            if (!DeleteImage(ImageDir))
            {
                return null;
            }
            return await UploadImage(image);
        }

        public bool DeleteImage(string image)
        {
            string path = Path.Combine(CurrentDir, image);
            if (!File.Exists(path))
            {
                _logger.LogError("Image Doesnt Exists");
                return false;
            }
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed To Delete Old Image exception :{Message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Failed To Delete Old Image Innerexception :{Message}", ex.InnerException.Message);
                }

                return false;
            }
        }

        public string HashImage(IFormFile image)
        {
            using (var sha256 = SHA256.Create())
            using (Stream stream = image.OpenReadStream())
            {
                byte[] bytes = sha256.ComputeHash(stream);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
