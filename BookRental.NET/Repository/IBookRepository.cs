using BookRental.NET.Models;

namespace BookRental.NET.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> UpdateAsync(Book book);
    }
}
