using System.Net;
using System.Net.Http.Json;
using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GenealogyApp.Tests.Integration
{
    public class PhotoControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PhotoControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPhotos_ReturnsOk()
        {
            // Arrange
            var memberId = Guid.NewGuid(); // Assure-toi que ce membre existe dans la base de test

            // Act
            var response = await _client.GetAsync($"/api/photo/{memberId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AddPhoto_ReturnsBadRequest_WhenNoFile()
        {
            var dto = new PhotoUploadDto
            {
                MemberId = Guid.NewGuid(),
                File = null // Simule l'absence de fichier
            };

            var response = await _client.PostAsJsonAsync("/api/photo/add", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
