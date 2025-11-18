using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string subFolder, CancellationToken cancellationToken);
        Task<List<string>> SaveFileAsync(List<IFormFile> files, string subFolder, CancellationToken cancellationToken);
        string GetFileUrl(string relativePath);
    }
}
