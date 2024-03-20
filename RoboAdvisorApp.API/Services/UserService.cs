using Microsoft.EntityFrameworkCore;
using RoboAdvisorApp.API.Data;
using RoboAdvisorApp.API.Models.Domain;
using RoboAdvisorApp.API.Models.DTO;
using RoboAdvisorApp.API.Services.Interface;
using BCrypt.Net;
using System;
using RoboAdvisorApp.API.Models.DTO.Response;

namespace RoboAdvisorApp.API.Services
{
    public class UserService: IUserService
    {
        private readonly RoboAppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(RoboAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            // Check if user exists and if password matches
            if (user == null || !VerifyPassword(user.PasswordHash, password))
            {
                return null;
            }

            return new AuthenticationResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                PhoneNo = user.PhoneNo,
                UserId = user.Id
            };
        }

        public async Task<RegistrationResponse> RegisterAsync(UserDto userDto)
        {
            // Check if username or email already exists
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username || u.Email == userDto.Email))
            {
                throw new ApplicationException("Username or email already exists");
            }

            // Map UserDTO to User entity
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Username = userDto.Username,
                PhoneNo = userDto.PhoneNo,
            };

            // Hash password before saving
            user.PasswordHash = HashPassword(userDto.Password);


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegistrationResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                PhoneNo = user.PhoneNo
            };
        }

        // Helper method to hash password using BCrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Helper method to verify password using BCrypt
        private bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

   
    }
}
