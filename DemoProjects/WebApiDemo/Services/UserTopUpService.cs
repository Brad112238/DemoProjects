using WebApiDemo.Interfaces;
using WebApiDemo.Models.TestDb;

namespace WebApiDemo.Services
{
    public class UserTopUpService : IUserTopUpService
    {
        private readonly TestContext _context;

        public UserTopUpService(TestContext context)
        {
            _context = context;
        }

        public IQueryable<UserTopUp> Query()
        {
            return _context.UserTopUps;
        }

        public void Add(UserTopUp entity)
        {
            _context.UserTopUps.Add(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
