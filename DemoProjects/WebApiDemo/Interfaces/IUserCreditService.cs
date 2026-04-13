using WebApiDemo.Models.TestDb;

namespace WebApiDemo.Interfaces
{
    public interface IUserCreditService
    {
        IQueryable<UserCredit> Query();

        IQueryable<UserCredit> QueryTracking();

        Task SaveChangesAsync();
    }
}
