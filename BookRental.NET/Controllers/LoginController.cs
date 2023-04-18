using AutoMapper;
using BookRental.NET.Data;
using BookRental.NET.Models;
using BookRental.NET.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookRental.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BookRentalDbContext _bookRentalDbContext;
        private readonly IMapper _mapper;

        public LoginController(BookRentalDbContext bookRentalDbContext, IMapper mapper)
        {
            _bookRentalDbContext = bookRentalDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            IEnumerable<User> userList = await _bookRentalDbContext.Users.ToListAsync();
            return Ok(_mapper.Map<List<UserDTO>>(userList));
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var user = await _bookRentalDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTOCreate createDTO)
        {
            // Email address is not unique
            if (await _bookRentalDbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == createDTO.Email.ToLower()) != null)
            {
                ModelState.AddModelError("Duplicate Error", "Email address is already in use");
                return BadRequest(ModelState);
            }

            if (createDTO == null) return BadRequest();

            User user = _mapper.Map<User>(createDTO);

            await _bookRentalDbContext.Users.AddAsync(user);
            await _bookRentalDbContext.SaveChangesAsync();

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id == 0) return BadRequest();

            var user = await _bookRentalDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null) return NotFound();

            _bookRentalDbContext.Users.Remove(user);
            await _bookRentalDbContext.SaveChangesAsync();

            return NoContent();
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "EditUser")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTOUpdate updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id) return BadRequest();

            User user = _mapper.Map<User>(updateDTO);

            _bookRentalDbContext.Users.Update(user);
            await _bookRentalDbContext.SaveChangesAsync();

            return NoContent();
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "EditUserPartial")]
        public async Task<IActionResult> UpdateUserPartial(int id, JsonPatchDocument<UserDTOUpdate> patchDTO)
        {
            if (patchDTO == null || id == 0) return BadRequest();

            var user = await _bookRentalDbContext.Users.AsNoTracking().FirstOrDefaultAsync(userDTO => userDTO.Id == id);

            UserDTOUpdate updateDTO = _mapper.Map<UserDTOUpdate>(user);

            if (user == null) return NotFound();

            patchDTO.ApplyTo(updateDTO, ModelState);

            User model = _mapper.Map<User>(updateDTO);

            _bookRentalDbContext.Users.Update(model);
            await _bookRentalDbContext.SaveChangesAsync();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }
    }
}
