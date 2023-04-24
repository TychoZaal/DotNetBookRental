using AutoMapper;
using BookRental.NET.Models;
using BookRental.NET.Models.Dto;

namespace BookRental.NET
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Users
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<UserDTO, UserDTOCreate>().ReverseMap();
            CreateMap<User, UserDTOCreate>().ReverseMap();
            CreateMap<UserDTO, UserDTOUpdate>().ReverseMap();
            CreateMap<User, UserDTOUpdate>().ReverseMap();

            // Books
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<BookDTO, BookDTOCreate>().ReverseMap();
            CreateMap<Book, BookDTOCreate>().ReverseMap();
            CreateMap<BookDTO, BookDTOUpdate>().ReverseMap();
            CreateMap<Book, BookDTOUpdate>().ReverseMap();

            // Reservations
            CreateMap<Reservation, ReservationDTO>().ReverseMap();
            CreateMap<ReservationDTO, ReservationDTOCreate>().ReverseMap();
            CreateMap<Reservation, ReservationDTOCreate>().ReverseMap();
            CreateMap<ReservationDTO, ReservationDTOUpdate>().ReverseMap();
            CreateMap<Reservation, ReservationDTOUpdate>().ReverseMap();

            // Loans
            CreateMap<Loan, LoanDTO>().ReverseMap();
            CreateMap<LoanDTO, LoanDTOCreate>().ReverseMap();
            CreateMap<Loan, LoanDTOCreate>().ReverseMap();
            CreateMap<LoanDTO, LoanDTOUpdate>().ReverseMap();
            CreateMap<Loan, LoanDTOUpdate>().ReverseMap();
        }
    }
}
