using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string subFolder, CancellationToken cancellationToken);
    }
}
