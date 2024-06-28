﻿using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<User> RegisterUser(UserRegistrationDto userDto);
        Task<string> LoginUser(UserLoginDto userDto);
    }
}
