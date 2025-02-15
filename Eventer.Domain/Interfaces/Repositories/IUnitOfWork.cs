﻿using Eventer.Domain.Models;

namespace Eventer.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        IRepository<EventRegistration> Registrations { get; }
        Task<int> SaveChangesAsync();
    }
}
