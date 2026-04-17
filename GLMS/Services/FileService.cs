using Microsoft.AspNetCore.Http;

namespace GLMS.Web.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _allowedExtensions = [".pdf"];

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void ValidatePdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("A PDF file is required.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Only .pdf files are allowed.");
        }

        public async Task<string> SaveContractPdfAsync(IFormFile file)
        {
            ValidatePdf(file);

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "contracts");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/contracts/{fileName}";
        }
    }
}