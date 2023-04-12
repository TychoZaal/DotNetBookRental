using BookRental.NET.Models.Dto;

namespace BookRental.NET.Models
{
    public static class UserDatabase
    {
        public static List<UserDTO> userList = new List<UserDTO>
        {
            new UserDTO{Id=1, Name="Tycho Zaal", Email="TychoZaal@gmail.com", Location="Hoorn", PhoneNumber="0643118441", StartingDate=DateTime.Now },
            new UserDTO{Id=2, Name="Guido Zaal", Email="GuidoZaal@gmail.com", Location="Hoorn", PhoneNumber="0612345678", StartingDate=DateTime.Now }
        };
    }
}
