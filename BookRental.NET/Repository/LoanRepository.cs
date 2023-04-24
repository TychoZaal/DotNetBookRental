using BookRental.NET.Data;
using BookRental.NET.Models;

namespace BookRental.NET.Repository
{
    public class LoanRepository : Repository<Loan>, ILoanRepository
    {
        private readonly BookRentalDbContext _dbContext;

        public LoanRepository(BookRentalDbContext context) : base(context)
        {
            this._dbContext = context;
        }

        public async Task<Loan> EndLoanAsync(Loan loan)
        {
            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();
            return loan;
        }
    }
}
