using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto registerDto);
        Task<TokenDto> LoginAsync(LoginUserDto loginDto);
        Task<UserDto> GetUserByIdAsync(Guid userId);
    }
}
