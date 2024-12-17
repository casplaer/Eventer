using Eventer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Eventer.Application.Services
{
    public class ImageService : IImageService
    {
        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string uploadPath, string baseUrl, string imageType)
        {
            var imagePaths = new List<string>();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imagePaths.Add($"{baseUrl}/uploads/{imageType}/{uniqueFileName}");
                }
            }

            return imagePaths;
        }

        public void DeleteImages(IEnumerable<string> imagePaths, string uploadPath)
        {
            if (imagePaths == null)
                return;

            foreach (var imagePath in imagePaths)
            {
                var fullPath = Path.Combine(uploadPath, Path.GetFileName(imagePath));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }

        public async Task<List<string>> UpdateImagesAsync(IEnumerable<IFormFile> newImages,
                IEnumerable<string> existingImages,
                IEnumerable<string> removedImages,
                string uploadPath,
                string baseUrl,
                string imageType)
        {
            var imagePaths = new List<string>();

            foreach (var image in newImages)
            {
                if (image.Length > 0)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imagePaths.Add($"{baseUrl}/uploads/{imageType}/{uniqueFileName}");
                }
            }

            if (existingImages != null && existingImages.Any())
            {
                foreach (var existingImage in existingImages)
                {
                    if (!string.IsNullOrEmpty(existingImage))
                    {
                        var fileName = Path.GetFileName(existingImage);
                        imagePaths.Add($"{baseUrl}/uploads/{imageType}/{fileName}");
                    }
                }
            }

            if (removedImages != null && removedImages.Any())
            {
                foreach (var image in removedImages)
                {
                    var imagePath = Path.Combine(uploadPath, Path.GetFileName(image));
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
            }

            return imagePaths;
        }
    }

}
