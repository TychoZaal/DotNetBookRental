using BookRental.NET.Data;
using BookRental.NET.Models;
using BookRental.NET.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BookRentalDbContext _bookRentalDbContext;

        public LoginController(BookRentalDbContext bookRentalDbContext)
        {
            _bookRentalDbContext = bookRentalDbContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            return Ok(_bookRentalDbContext.Users.ToList());
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var user = _bookRentalDbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> CreateUser([FromBody] UserDTO userDTO)
        {
            // Email address is not unique
            if (_bookRentalDbContext.Users.FirstOrDefault(u => u.Email.ToLower() == userDTO.Email.ToLower()) != null)
            {
                ModelState.AddModelError("Duplicate Error", "Email address is already in use");
                return BadRequest(ModelState);
            }

            if (userDTO == null) return BadRequest();

            if (userDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            User user = new User()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Location = userDTO.Location,
                Password = userDTO.Password,
                PhoneNumber = userDTO.PhoneNumber,
                StartingDate = userDTO.StartingDate,
                IsAdmin = userDTO.IsAdmin
            };

            _bookRentalDbContext.Users.Add(user);
            _bookRentalDbContext.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = userDTO.Id }, userDTO);
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            if (id == 0) return BadRequest();

            var user = _bookRentalDbContext.Users.FirstOrDefault(user => user.Id == id);

            if (user == null) return NotFound();

            _bookRentalDbContext.Users.Remove(user);

            return NoContent();
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "EditUser")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null || id != userDTO.Id) return BadRequest();

            User user = new User()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Location = userDTO.Location,
                Password = userDTO.Password,
                PhoneNumber = userDTO.PhoneNumber,
                StartingDate = userDTO.StartingDate,
                IsAdmin = userDTO.IsAdmin
            };

            _bookRentalDbContext.Users.Update(user);
            _bookRentalDbContext.SaveChanges();

            return NoContent();
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "EditUserPartial")]
        public IActionResult UpdateUserPartial(int id, JsonPatchDocument<UserDTO> patchDTO)
        {
            if (patchDTO == null || id == 0) return BadRequest();

            var user = _bookRentalDbContext.Users.FirstOrDefault(userDTO => userDTO.Id == id);

            UserDTO userDTO = new UserDTO()
            {
                Name = user.Name,
                Email = user.Email,
                Location = user.Location,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                StartingDate = user.StartingDate,
                IsAdmin = user.IsAdmin
            };

            if (user == null) return NotFound();

            patchDTO.ApplyTo(userDTO, ModelState);

            User patchedUser = new User()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Location = userDTO.Location,
                Password = userDTO.Password,
                PhoneNumber = userDTO.PhoneNumber,
                StartingDate = userDTO.StartingDate,
                IsAdmin = userDTO.IsAdmin
            };

            _bookRentalDbContext.Users.Update(patchedUser);
            _bookRentalDbContext.SaveChanges();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }
    }
}
