// UserService.cs

using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DecentraCloud.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenHelper _tokenHelper;

        public UserService(IUserRepository userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
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

            await _userRepository.RegisterUser(user);
            return user;
        }

        public async Task<User> AuthenticateUser(UserLoginDto userDto)
        {
            var user = await _userRepository.GetUserByEmail(userDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                throw new Exception("Invalid email or password");
            }

            return user;
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _userRepository.GetUserById(userId);
        }
    }
}
