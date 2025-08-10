using Xunit;
using Moq;
using FluentAssertions;
using GenealogyApp.API.Controllers;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GenealogyApp.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Username = "testuser",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Password = "password"
            };

            var expectedUser = new UserDto
            {
                UserId = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email
            };

            _mockService.Setup(s => s.RegisterAsync(dto)).ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUserExists()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Username = "existinguser",
                Email = "existing@example.com",
                PhoneNumber = "1234567890",
                Password = "password"
            };

            _mockService.Setup(s => s.RegisterAsync(dto)).ReturnsAsync((UserDto?)null);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
