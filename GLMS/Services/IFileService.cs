using Microsoft.AspNetCore.Http;

namespace GLMS.Web.Services
{
    public interface IFileService
    {
        Task<string> SaveContractPdfAsync(IFormFile file);
        void ValidatePdf(IFormFile file);
    }
}