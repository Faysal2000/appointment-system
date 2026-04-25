using AppointmentSystem.DTOs.Auth;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interfaces;
using AppointmentSystem.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppointmentSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Check if email exists
            var users = await _unitOfWork.Users.GetAllAsync();
            if (users.Any(u => u.Email == dto.Email))
                throw new Exception("Email already exists");

            // Create User
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                Address = dto.Address,
                RoleId = dto.RoleId
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // If Role is Doctor (RoleId = 2), create Doctor record
            if (dto.RoleId == 2)
            {
                var doctor = new Doctor
                {
                    UserId = user.Id,
                    ExperienceYears = 0
                };
                await _unitOfWork.Doctors.AddAsync(doctor);
                await _unitOfWork.SaveChangesAsync();
            }

            var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId);
            var token = GenerateJwtToken(user, role?.Name ?? "User");

            return new AuthResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = role?.Name ?? "User",
                ExpiresAt = DateTime.UtcNow.AddDays(
                    _configuration.GetValue<int>("JwtSettings:ExpiryInDays"))
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            // Find user by email
            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null) return null;

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return null;

            var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId);
            var token = GenerateJwtToken(user, role?.Name ?? "User");

            return new AuthResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = role?.Name ?? "User",
                ExpiresAt = DateTime.UtcNow.AddDays(
                    _configuration.GetValue<int>("JwtSettings:ExpiryInDays"))
            };
        }

        private string GenerateJwtToken(User user, string roleName)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, roleName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(
                    _configuration.GetValue<int>("JwtSettings:ExpiryInDays")),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}