using BookRental.NET.Data;
using BookRental.NET.Models;

namespace BookRental.NET.Repository
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        private readonly BookRentalDbContext _dbContext;

        public ReservationRepository(BookRentalDbContext context) : base(context)
        {
            this._dbContext = context;
        }

        public async Task<Reservation> UpdateAsync(Reservation reservation, bool toApprove)
        {
            reservation.Status = toApprove ? "APPROVED" : "DENIED";

            _dbContext.Reservations.Update(reservation);
            await _dbContext.SaveChangesAsync();

            return reservation;
        }
    }
}
