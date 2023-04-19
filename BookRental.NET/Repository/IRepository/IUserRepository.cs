using BookRental.NET.Models;
using System.Linq.Expressions;

namespace BookRental.NET.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(Expression<Func<User, bool>> filter = null);
        Task<User> GetAsync(Expression<Func<User, bool>> filter = null, bool track = true);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveAsync(User user);
        Task SaveAsync();
    }
}
