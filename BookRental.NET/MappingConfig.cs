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
        }
    }
}
