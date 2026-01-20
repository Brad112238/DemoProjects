using Microsoft.EntityFrameworkCore;
using WebApiDemo.Interfaces;
using WebApiDemo.Models.TestDb;

namespace WebApiDemo.Services
{
    public class UserCreditService : IUserCreditService
    {
        private readonly TestContext _context;

        public UserCreditService(TestContext context)
        {
            _context = context;
        }

        public IQueryable<UserCredit> Query()
        {
            return _context.UserCredits.AsNoTracking();
        }
    }
}
