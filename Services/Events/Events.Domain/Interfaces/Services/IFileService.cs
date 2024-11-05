using Microsoft.AspNetCore.Http;

namespace Events.Domain.Interfaces.Services
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(IFormFile? imageFile, string[] allowedFileExtensions);
        void DeleteFile(string? fileNameWithExtension);
    }
}
