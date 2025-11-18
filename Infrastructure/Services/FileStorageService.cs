using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetFileUrl(string relativePath)
        {
            return relativePath;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subFolder, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", subFolder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            return $"/images/{subFolder}/{uniqueFileName}";
        }

        public async Task<List<string>> SaveFileAsync(List<IFormFile> files, string subFolder, CancellationToken cancellationToken)
        {
            var uploadedUrls = new List<string>();
            var uploadedPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", subFolder);
            if (!Directory.Exists(uploadedPath))
            {
                Directory.CreateDirectory(uploadedPath);
            }

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                string uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
                string filePath = Path.Combine(uploadedPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                uploadedUrls.Add($"/images/{subFolder}/{uniqueFileName}");
            }
            return uploadedUrls;
        }
    }
}
