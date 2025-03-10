using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using BCrypt.Net;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException("Invalid user data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ValidationException("Email and password are required.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser is not null)
            {
                throw new ConflictException("User already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hashedPassword
            };

            await _userRepository.CreateAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<TokenDto> LoginAsync(LoginUserDto loginDto)
        {
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                throw new ValidationException("Email and password are required.");
            }

            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedException("Invalid credentials.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new TokenDto(token, DateTime.UtcNow.AddHours(2));
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ValidationException("Invalid user ID.");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                throw new NotFoundException("User not found.");
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
