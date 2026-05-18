using Microsoft.AspNetCore.Http;

namespace GLMS.Api.Services
{
    public interface IFileService
    {
        Task<string> SaveContractPdfAsync(IFormFile file);
    }
}