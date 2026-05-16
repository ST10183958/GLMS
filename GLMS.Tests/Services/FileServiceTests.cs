using GLMS.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace GLMS.Tests
{
    public class FileServiceTests
    {
        [Fact]
        public async Task UploadExe_ShouldThrow()
        {
            var env = new Mock<IWebHostEnvironment>();

            var service = new FileService(env.Object);

            var file = new Mock<IFormFile>();

            file.Setup(f => f.FileName)
                .Returns("virus.exe");

            file.Setup(f => f.Length)
                .Returns(100);

            await Assert.ThrowsAsync<Exception>(() =>
                service.SaveContractPdfAsync(file.Object));
        }

        [Fact]
        public async Task UploadPdfExe_ShouldThrow()
        {
            var env = new Mock<IWebHostEnvironment>();

            var service = new FileService(env.Object);

            var file = new Mock<IFormFile>();

            file.Setup(f => f.FileName)
                .Returns("contract.pdf.exe");

            file.Setup(f => f.Length)
                .Returns(100);

            await Assert.ThrowsAsync<Exception>(() =>
                service.SaveContractPdfAsync(file.Object));
        }

        [Fact]
        public async Task UploadEmptyFile_ShouldThrow()
        {
            var env = new Mock<IWebHostEnvironment>();

            var service = new FileService(env.Object);

            var file = new Mock<IFormFile>();

            file.Setup(f => f.FileName)
                .Returns("contract.pdf");

            file.Setup(f => f.Length)
                .Returns(0);

            await Assert.ThrowsAsync<Exception>(() =>
                service.SaveContractPdfAsync(file.Object));
        }
    }
}