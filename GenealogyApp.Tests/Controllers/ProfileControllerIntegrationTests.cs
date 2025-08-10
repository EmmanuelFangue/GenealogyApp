using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using GenealogyApp.Tests.Integration;
using GenealogyApp.Application.DTOs;
using System.Net;


namespace GenealogyApp.Tests.Controllers
{

    public class ProfileControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProfileControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProfile_ShouldReturnClaims_WhenAuthenticated()
        {
            // Enregistrement
            var registerDto = new RegisterUserDto
            {
                Username = "integrationuser",
                Email = "integration@example.com",
                PhoneNumber = "1234567890",
                Password = "securepassword"
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/User/register", registerDto);
            var registerContent = await registerResponse.Content.ReadAsStringAsync();
            Assert.True(registerResponse.IsSuccessStatusCode, $"Register failed: {registerContent}");
            registerResponse.EnsureSuccessStatusCode();

            // Connexion
            var loginDto = new LoginDto
            {
                UsernameOrEmail = "integrationuser",
                Password = "securepassword"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/User/login", loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            Assert.True(loginResponse.IsSuccessStatusCode, $"Login failed: {loginContent}");

            var token = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!.Token);

            // Appel à /api/Profile
            var response = await _client.GetAsync("/api/Profile");
            response.EnsureSuccessStatusCode();

            var profile = await response.Content.ReadFromJsonAsync<ProfileResponseDto>();
            profile.Username.Should().Be("integrationuser");
        }

        [Fact]
        public async Task GetProfile_ShouldReturn401_WhenNotAuthenticated()
        {
            var response = await _client.GetAsync("/api/Profile");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }



    }

}
