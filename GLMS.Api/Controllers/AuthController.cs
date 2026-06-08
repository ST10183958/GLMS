using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using GLMS.Api.Data;
using GLMS.Api.DTOs;
using GLMS.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GLMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Username))
                    return BadRequest("Username is required.");

                if (string.IsNullOrWhiteSpace(dto.Email))
                    return BadRequest("Email is required.");

                if (string.IsNullOrWhiteSpace(dto.Password))
                    return BadRequest("Password is required.");

                var exists = await _context.Users
                    .AnyAsync(u => u.Username == dto.Username);

                if (exists)
                {
                    return BadRequest("Username already exists.");
                }

                var hashedPassword =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    PasswordHash = hashedPassword,
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Registration successful."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u =>
                        u.Username == dto.Username);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                var passwordValid =
                    BCrypt.Net.BCrypt.Verify(
                        dto.Password,
                        user.PasswordHash);

                if (!passwordValid)
                {
                    return Unauthorized("Invalid username or password.");
                }

                var token = GenerateJwtToken(user);

                return Ok(new
                {
                    Token = token,
                    Username = user.Username,
                    Role = user.Role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }
        

        private string GenerateJwtToken(User user)
        {
            var jwtKey =
                _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new Exception(
                    "Jwt:Key is missing from configuration.");
            }

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Name,
                    user.Username),

                new Claim(
                    ClaimTypes.Role,
                    user.Role),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer:
                        _configuration["Jwt:Issuer"],

                    audience:
                        _configuration["Jwt:Audience"],

                    claims: claims,

                    expires:
                        DateTime.UtcNow.AddHours(2),

                    signingCredentials:
                        credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}