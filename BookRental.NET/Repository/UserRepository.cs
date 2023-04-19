using BookRental.NET.Data;
using BookRental.NET.Models;
using BookRental.NET.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookRental.NET.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BookRentalDbContext _dbContext;

        public UserRepository(BookRentalDbContext context)
        {
            this._dbContext = context;
        }

        public async Task CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await SaveAsync();
        }

        public async Task<User> GetAsync(Expression<Func<User, bool>> filter = null, bool track = true)
        {
            IQueryable<User> query = _dbContext.Users;

            if (!track)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllAsync(Expression<Func<User, bool>> filter = null)
        {
            IQueryable<User> query = _dbContext.Users;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(User user)
        {
            _dbContext.Users.Remove(user);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await SaveAsync();
        }
    }
}
