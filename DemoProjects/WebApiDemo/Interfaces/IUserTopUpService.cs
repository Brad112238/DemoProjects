using WebApiDemo.Models.TestDb;

namespace WebApiDemo.Interfaces
{
    public interface IUserTopUpService
    {
        IQueryable<UserTopUp> Query();

        void Add(UserTopUp entity);

        Task SaveChangesAsync();
    }
}
