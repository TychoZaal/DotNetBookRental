using BookRental.NET.Data;
using BookRental.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookRental.NET.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly BookRentalDbContext _dbContext;

        public UserRepository(BookRentalDbContext context) : base(context)
        {
            this._dbContext = context;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
