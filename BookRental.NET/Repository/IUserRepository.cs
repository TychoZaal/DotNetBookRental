using BookRental.NET.Models;
using System.Linq.Expressions;

namespace BookRental.NET.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> UpdateAsync(User user);
    }
}
