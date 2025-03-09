using Application.Interfaces;
using Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.Database.GetCollection<User>("Users");
        }

        public async Task CreateAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }
}
