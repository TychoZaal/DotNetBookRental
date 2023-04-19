using BookRental.NET.Data;
using BookRental.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookRental.NET.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly BookRentalDbContext _dbContext;

        public BookRepository(BookRentalDbContext context) : base(context)
        {
            this._dbContext = context;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
            return book;
        }
    }
}
