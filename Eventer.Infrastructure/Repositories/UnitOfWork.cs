using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;

namespace Eventer.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventerDbContext _context;
        private IEventRepository? _eventRepository;
        private ICategoryRepository? _categoryRepository;
        private IUserRepository? _userRepository;
        private IRepository<EventRegistration> _eventRegistrationRepository;

        public UnitOfWork(
            EventerDbContext context,
            IEventRepository eventRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IRepository<EventRegistration> eventRegistrationRepository)
        {
            _context = context;
            _eventRepository = eventRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _eventRegistrationRepository = eventRegistrationRepository;
        }

        public IEventRepository Events => _eventRepository;
        public ICategoryRepository Categories => _categoryRepository;
        public IUserRepository Users => _userRepository;
        public IRepository<EventRegistration> Registrations => _eventRegistrationRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
