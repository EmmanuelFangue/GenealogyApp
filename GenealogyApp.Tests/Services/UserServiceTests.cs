using Xunit;
using FluentAssertions;
using GenealogyApp.Application.Services;
using GenealogyApp.Application.DTOs;
using GenealogyApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenealogyApp.Tests.Services
{
    public class UserServiceTests
    {
        private readonly GenealogyDbContext _db;
        private readonly UserService _service;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<GenealogyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB par test
                .Options;

            _db = new GenealogyDbContext(options);

            // Appliquer les configurations Fluent API
            _db.Database.EnsureCreated();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "testkey1234567890" },
                    { "Jwt:Issuer", "TestIssuer" },
                    { "Jwt:Audience", "TestAudience" }
                })
                .Build();

            _service = new UserService(_db, config);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_WhenValid()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Username = "newuser",
                Email = "new@example.com",
                PhoneNumber = "1234567890",
                Password = "securepassword"
            };

            // Act
            var result = await _service.RegisterAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result!.Username.Should().Be(dto.Username);
            result.Email.Should().Be(dto.Email);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnNull_WhenUserAlreadyExists()
        {
            // Arrange
            var existing = new RegisterUserDto
            {
                Username = "existinguser",
                Email = "existing@example.com",
                PhoneNumber = "1234567890",
                Password = "password"
            };

            await _service.RegisterAsync(existing);

            // Act
            var result = await _service.RegisterAsync(existing);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Username = "loginuser",
                Email = "login@example.com",
                PhoneNumber = "1234567890",
                Password = "mypassword"
            };

            await _service.RegisterAsync(dto);

            var loginDto = new LoginDto
            {
                UsernameOrEmail = "loginuser",
                Password = "mypassword"
            };

            // Act
            var result = await _service.LoginAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result!.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            var dto = new RegisterUserDto
            {
                Username = "badlogin",
                Email = "bad@example.com",
                PhoneNumber = "1234567890",
                Password = "correctpass"
            };

            await _service.RegisterAsync(dto);

            var loginDto = new LoginDto
            {
                UsernameOrEmail = "badlogin",
                Password = "wrongpass"
            };

            var result = await _service.LoginAsync(loginDto);

            result.Should().BeNull();
        }
    }
}
