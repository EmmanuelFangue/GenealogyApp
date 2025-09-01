using GenealogyApp.Application.DTOs;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;
using GenealogyApp.Infrastructure.Services;
using Moq;

namespace GenealogyApp.Tests.Controllers
{
    public class PhotoServiceTests
    {
        private readonly Mock<GenealogyDbContext> _mockDbContext;
        private readonly PhotoService _photoService;

        public PhotoServiceTests()
        {
            _mockDbContext = new Mock<GenealogyDbContext>();
            _photoService = new PhotoService(_mockDbContext.Object);
        }

        [Fact]
        public async Task AddPhotoAsync_ShouldReturnNull_WhenMemberNotFound()
        {
            // Arrange
            var dto = new PhotoUploadDto { MemberId = Guid.NewGuid(), File = null };
            _mockDbContext.Setup(db => db.FamilyMembers.FindAsync(dto.MemberId))
                          .ReturnsAsync((FamilyMember)null);

            // Act
            var result = await _photoService.AddPhotoAsync(dto);

            // Assert
            Assert.Null(result);
        }
    }

}
