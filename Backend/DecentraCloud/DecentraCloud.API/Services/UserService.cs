// Services/UserService.cs

using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DecentraCloud.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User> RegisterUser(UserRegistrationDto userDto)
        {
            var existingUser = await _userRepository.GetUserByEmail(userDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists");
            }

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            return await _userRepository.RegisterUser(user);
        }

        public async Task<string> LoginUser(UserLoginDto userDto)
        {
            var user = await _userRepository.GetUserByEmail(userDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                throw new Exception("Invalid email or password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
