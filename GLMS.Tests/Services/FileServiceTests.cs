using GLMS.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace GLMS.Tests.Services
{
    public class FileServiceTests
    {
        [Fact]
        public void ValidatePdf_ShouldThrow_WhenFileIsExe()
        {
            // Arrange
            var envMock = new Mock<IWebHostEnvironment>();
            var service = new FileService(envMock.Object);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("malware.exe");
            fileMock.Setup(f => f.Length).Returns(100);

            // Act + Assert
            Assert.Throws<InvalidOperationException>(() =>
                service.ValidatePdf(fileMock.Object));
        }

        [Fact]
        public void ValidatePdf_ShouldPass_WhenFileIsPdf()
        {
            // Arrange
            var envMock = new Mock<IWebHostEnvironment>();
            var service = new FileService(envMock.Object);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("contract.pdf");
            fileMock.Setup(f => f.Length).Returns(100);

            // Act
            service.ValidatePdf(fileMock.Object);

            // Assert
            Assert.True(true);
        }
    }
}