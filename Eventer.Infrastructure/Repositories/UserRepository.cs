using Eventer.Domain.Interfaces.Repositories;
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
            return _context.Users
                .Include(u=>u.EventRegistrations)
                .ThenInclude(er => er.Event)
                .FirstOrDefaultAsync(u=>u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        public async Task<User?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == token, cancellationToken);
        }
    }
}
