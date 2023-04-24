using BookRental.NET.Models;

namespace BookRental.NET.Repository
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<Loan> EndLoanAsync(Loan loan);
    }
}
