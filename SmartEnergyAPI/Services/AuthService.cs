using SmartEnergyAPI.DTOs;
using SmartEnergyAPI.Helpers;
using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;

namespace SmartEnergyAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
                throw new Exception("Denne e-mail er allerede registreret.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Trader",
                Balance = 10000
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Forkert e-mail eller adgangskode.");

            if (!user.IsActive)
                throw new Exception("Din konto er deaktiveret.");

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        private async Task<AuthResponseDto> GenerateAuthResponse(User user)
        {
            var accessToken = _jwtHelper.GenerateAccessToken(user);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}