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
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            List<UserDTO> user = UserDatabase.userList;

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUser(int id)
        {
            if (id == 0) return BadRequest();

            UserDTO user = UserDatabase.userList.FirstOrDefault(u => u.Id == id);

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
            if (UserDatabase.userList.FirstOrDefault(u => u.Email.ToLower() == userDTO.Email.ToLower()) != null)
            {
                ModelState.AddModelError("Duplicate Error", "Email address is already in use");
                return BadRequest(ModelState);
            }

            if (userDTO == null) return BadRequest();

            if (userDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            userDTO.Id = UserDatabase.userList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

            UserDatabase.userList.Add(userDTO);

            return CreatedAtRoute("GetUser", new { id = userDTO.Id }, userDTO);
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            if (id == 0) return BadRequest();

            UserDTO user = UserDatabase.userList.FirstOrDefault(user => user.Id == id);

            if (user == null) return NotFound();

            UserDatabase.userList.Remove(user);

            return NoContent();
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "EditUser")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null || id != userDTO.Id) return BadRequest();

            UserDTO user = UserDatabase.userList.FirstOrDefault(userDTO => userDTO.Id == id);

            if (user == null) return NotFound();

            user.Name = userDTO.Name;
            user.Email = userDTO.Email;
            user.Location = userDTO.Location;
            user.PhoneNumber = userDTO.PhoneNumber;

            return NoContent();
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "EditUserPartial")]
        public IActionResult UpdateUserPartial(int id, JsonPatchDocument<UserDTO> patchDTO)
        {
            if (patchDTO == null || id == 0) return BadRequest();

            UserDTO user = UserDatabase.userList.FirstOrDefault(userDTO => userDTO.Id == id);

            if (user == null) return NotFound();

            patchDTO.ApplyTo(user, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }
    }
}
