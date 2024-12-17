﻿using Eventer.Domain.Interfaces.Repositories;
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

        public async Task<User?> GetByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        }


        public override Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Users.Include(u=>u.EventRegistrations).FirstOrDefaultAsync(u=>u.Id == id, cancellationToken);
        }

        public async Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
            if( user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<User> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == token, cancellationToken);
            if(user == null)
            {
                return null;
            }

            return user;
        }
    }
}
