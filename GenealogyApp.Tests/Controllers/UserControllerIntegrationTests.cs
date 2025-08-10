using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using GenealogyApp.Application.DTOs;
using GenealogyApp.Tests.Integration;

namespace GenealogyApp.Tests.Controllers
{
    public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UserControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturnUser_WhenValid()
        {
            var dto = new RegisterUserDto
            {
                Username = "integrationuser",
                Email = "integration@example.com",
                PhoneNumber = "1234567890",
                Password = "securepassword"
            };

            var response = await _client.PostAsJsonAsync("/api/User/register", dto);

            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            user.Should().NotBeNull();
            user!.Username.Should().Be(dto.Username);
        }
    }
}
