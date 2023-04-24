using BookRental.NET.Models;

namespace BookRental.NET.Repository
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Reservation> UpdateAsync(Reservation reservation, bool toApprove);
    }
}
