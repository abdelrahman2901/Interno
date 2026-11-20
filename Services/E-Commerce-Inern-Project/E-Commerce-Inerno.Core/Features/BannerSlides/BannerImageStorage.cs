using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace E_Commerce_Inerno.Core.Features.BannerSlides
{
    /// <summary>
    /// Shared by the create, update and delete banner handlers, so it stays in one place
    /// instead of being duplicated across them.
    /// </summary>
    public class BannerImageStorage
    {
        public BannerImageStorage(IWebHostEnvironment env)
        {
            // WebRootPath is null until wwwroot exists on disk, which is the case on a fresh container.
            CurrentDir = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"), "BannerImages");
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

        public bool DeleteImage(string ImagePath)
        {
            if (!File.Exists(ImagePath))
            {
                return false;
            }
            try
            {
                File.Delete(ImagePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string HashImage(IFormFile image)
        {
            using var sha256 = SHA256.Create();
            using Stream stream = image.OpenReadStream();
            byte[] hashBytes = sha256.ComputeHash(stream);
            return Convert.ToHexStringLower(hashBytes);
        }
    }
}
