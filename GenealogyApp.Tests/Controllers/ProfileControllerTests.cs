using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using GenealogyApp.API.Controllers;
using GenealogyApp.Application.DTOs;


namespace GenealogyApp.Tests.Controllers
{

    public class ProfileControllerTests
    {
        [Fact]
        public void GetProfile_ShouldReturnUserClaims_WhenAuthenticated()
        {
            // Arrange
            var controller = new ProfileController();

            var userId = "123";
            var username = "testuser";

            var claims = new[]
            {
            new Claim("sub", userId),
            new Claim("username", username)
        };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            // Act

            var result = controller.GetProfile() as OkObjectResult;
            var value = result!.Value as ProfileResponseDto;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(userId, value!.Id);
            Assert.Equal(username, value.Username);

        }
    }

}
