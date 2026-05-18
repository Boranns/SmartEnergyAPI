using Moq;
using SmartEnergyAPI.DTOs;
using SmartEnergyAPI.Helpers;
using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;
using SmartEnergyAPI.Services;
using Microsoft.Extensions.Configuration;

namespace SmartEnergyAPI.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var jwtSection = new Mock<IConfigurationSection>();
            jwtSection.Setup(x => x["SecretKey"]).Returns("SmartEnergyAPI_SuperSecretKey_2026_MustBe32Chars!");
            jwtSection.Setup(x => x["Issuer"]).Returns("SmartEnergyAPI");
            jwtSection.Setup(x => x["Audience"]).Returns("SmartEnergyClient");
            jwtSection.Setup(x => x["AccessTokenExpireMinutes"]).Returns("15");
            jwtSection.Setup(x => x["RefreshTokenExpireDays"]).Returns("7");

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x.GetSection("JwtSettings")).Returns(jwtSection.Object);

            var jwtHelper = new JwtHelper(_configurationMock.Object);
            _authService = new AuthService(_userRepositoryMock.Object, jwtHelper);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            var password = "Test123!";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = 1,
                Username = "boran",
                Email = "borann@gmail.com",
                PasswordHash = hashedPassword,
                Role = "Trader",
                IsActive = true
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("borann@gmail.com"))
                .ReturnsAsync(user);

            var loginDto = new LoginDto
            {
                Email = "borann@gmail.com",
                Password = password
            };

            var result = await _authService.LoginAsync(loginDto);

            Assert.NotNull(result);
            Assert.NotNull(result.AccessToken);
            Assert.Equal("boran", result.Username);
            Assert.Equal("Trader", result.Role);
        }

        [Fact]
        public async Task Login_WithWrongPassword_ThrowsException()
        {
            var user = new User
            {
                Id = 1,
                Email = "borann@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct_password"),
                IsActive = true
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("borann@gmail.com"))
                .ReturnsAsync(user);

            var loginDto = new LoginDto
            {
                Email = "borann@gmail.com",
                Password = "wrong_password"
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _authService.LoginAsync(loginDto));
        }

        [Fact]
        public async Task Login_WithInactiveAccount_ThrowsException()
        {
            var user = new User
            {
                Id = 1,
                Email = "borann@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
                IsActive = false
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("borann@gmail.com"))
                .ReturnsAsync(user);

            var loginDto = new LoginDto
            {
                Email = "borann@gmail.com",
                Password = "Test123!"
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _authService.LoginAsync(loginDto));
        }

        [Fact]
        public async Task Register_WithExistingEmail_ThrowsException()
        {
            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("borann@gmail.com"))
                .ReturnsAsync(true);

            var registerDto = new RegisterDto
            {
                Username = "boran",
                Email = "borann@gmail.com",
                Password = "Test123!"
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _authService.RegisterAsync(registerDto));
        }

        [Fact]
        public async Task Register_WithNewEmail_ReturnsToken()
        {
            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("nybruger@gmail.com"))
                .ReturnsAsync(false);

            var registerDto = new RegisterDto
            {
                Username = "nybruger",
                Email = "nybruger@gmail.com",
                Password = "Test123!"
            };

            var result = await _authService.RegisterAsync(registerDto);

            Assert.NotNull(result);
            Assert.NotNull(result.AccessToken);
            Assert.Equal("nybruger", result.Username);
        }
    }
}
