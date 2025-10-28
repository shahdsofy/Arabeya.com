using Arabeya.Core.Domain.Contracts.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Arabeya.Infrastructure
{
    public class AttachmentService : IAttachmentService
    {
        private List<string> AllowedExtensions = new List<string>() { ".png", ".jpeg", ".jpg" };
        private const int _allowedMaxSize = 2_097_152;
        public async Task<string?> UploadAsync(IFormFile file, string folderName)
        {
            var extension = Path.GetExtension(file.FileName);

            if (!AllowedExtensions.Contains(extension)) return null;

            if (file.Length > _allowedMaxSize) return null;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(fileStream);

            return Path.Combine(folderName, fileName).Replace("\\", "/");
            

        }
    }
}
