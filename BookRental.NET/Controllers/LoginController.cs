using AutoMapper;
using BookRental.NET.Data;
using BookRental.NET.Models;
using BookRental.NET.Models.Dto;
using BookRental.NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookRental.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUserRepository _dbUser;
        private readonly IMapper _mapper;

        public LoginController(IUserRepository dbUser, IMapper mapper)
        {
            _dbUser = dbUser;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            try
            {
                IEnumerable<User> userList = await _dbUser.GetAllAsync();
                _response.Result = _mapper.Map<List<UserDTO>>(userList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUser(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var user = await _dbUser.GetAsync(u => u.Id == id);

                if (user == null) return NotFound();

                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody] UserDTOCreate createDTO)
        {
            try
            {
                // Email address is not unique
                if (await _dbUser.GetAsync(u => u.Email.ToLower() == createDTO.Email.ToLower()) != null)
                {
                    ModelState.AddModelError("Duplicate Error", "Email address is already in use");
                    return BadRequest(ModelState);
                }

                if (createDTO == null) return BadRequest();

                User user = _mapper.Map<User>(createDTO);

                await _dbUser.CreateAsync(user);
                await _dbUser.SaveAsync();

                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetUser", new { id = user.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteUser")]
        public async Task<ActionResult<APIResponse>> DeleteUser(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var user = await _dbUser.GetAsync(user => user.Id == id);

                if (user == null) return NotFound();

                await _dbUser.RemoveAsync(user);
                await _dbUser.SaveAsync();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "EditUser")]
        public async Task<ActionResult<APIResponse>> UpdateUser(int id, [FromBody] UserDTOUpdate updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id) return BadRequest();

                User user = _mapper.Map<User>(updateDTO);

                await _dbUser.UpdateAsync(user);
                await _dbUser.SaveAsync();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "EditUserPartial")]
        public async Task<ActionResult<APIResponse>> UpdateUserPartial(int id, JsonPatchDocument<UserDTOUpdate> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0) return BadRequest();

                var user = await _dbUser.GetAsync(userDTO => userDTO.Id == id, track: false);

                UserDTOUpdate updateDTO = _mapper.Map<UserDTOUpdate>(user);

                if (user == null) return NotFound();

                patchDTO.ApplyTo(updateDTO, ModelState);

                User model = _mapper.Map<User>(updateDTO);

                await _dbUser.UpdateAsync(model);
                await _dbUser.SaveAsync();

                if (!ModelState.IsValid) return BadRequest(ModelState);

                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
