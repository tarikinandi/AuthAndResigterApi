using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(Guid id);
    }
}
