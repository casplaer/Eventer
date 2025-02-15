﻿using Eventer.Application.Interfaces;
using Eventer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Validators
{
    public class UniqueFieldChecker : IUniqueFieldChecker
    {
        private readonly EventerDbContext _context;

        public UniqueFieldChecker(EventerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUniqueAsync<TEntity>(string fieldName, string value, Guid? excludeId = null)
            where TEntity : class
        {
            var query = _context.Set<TEntity>().AsQueryable();

            query = query.Where(e => EF.Property<string>(e, fieldName) == value);

            if (excludeId.HasValue)
            {
                query = query.Where(e => EF.Property<Guid>(e, "Id") != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}
