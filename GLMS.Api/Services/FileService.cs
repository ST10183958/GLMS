using Microsoft.AspNetCore.Http;

namespace GLMS.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveContractPdfAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("PDF file is required.");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");

            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "contracts"
            );

            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}.pdf";

            var fullPath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"/uploads/contracts/{fileName}";
        }
    }
}