using Eventer.Application.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EventerDbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
            {
                throw new Exception("No such user.");
            }
            
            return user;
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if( user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<User> GetByRefreshTokenAsync(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
            if(user == null)
            {
                return null;
            }

            return user;
        }
    }
}
