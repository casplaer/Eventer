using Microsoft.AspNetCore.Http;

namespace Eventer.Application.Interfaces.Services
{
    public interface IImageService
    {
        Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string uploadPath, string baseUrl, string imageType);
        Task<List<string>> UpdateImagesAsync(IEnumerable<IFormFile> newImages, 
                IEnumerable<string> existingImages,
                IEnumerable<string> removedImages,
                string uploadPath, 
                string baseUrl, 
                string imageType);
        void DeleteImages(IEnumerable<string> imagePaths, string uploadPath);
    }
}
