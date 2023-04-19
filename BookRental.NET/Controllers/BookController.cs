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
    public class BookController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IBookRepository _dbBook;
        private readonly IMapper _mapper;

        public BookController(IBookRepository dbBook, IMapper mapper)
        {
            _dbBook = dbBook;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<BookDTO>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBooks()
        {
            try
            {
                IEnumerable<Book> BookList = await _dbBook.GetAllAsync();
                _response.Result = _mapper.Map<List<BookDTO>>(BookList);
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

        [HttpGet("{id:int}", Name = "GetBook")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBook(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var Book = await _dbBook.GetAsync(u => u.Id == id);

                if (Book == null) return NotFound();

                _response.Result = _mapper.Map<BookDTO>(Book);
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
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateBook([FromBody] BookDTOCreate createDTO)
        {
            try
            {
                // Email address is not unique
                if (await _dbBook.GetAsync(u => u.Title.ToLower() == createDTO.Title.ToLower()) != null)
                {
                    ModelState.AddModelError("Duplicate Error", "Email address is already in use");
                    return BadRequest(ModelState);
                }

                if (createDTO == null) return BadRequest();

                Book Book = _mapper.Map<Book>(createDTO);

                await _dbBook.CreateAsync(Book);
                await _dbBook.SaveAsync();

                _response.Result = _mapper.Map<BookDTO>(Book);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetBook", new { id = Book.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteBook")]
        public async Task<ActionResult<APIResponse>> DeleteBook(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var Book = await _dbBook.GetAsync(Book => Book.Id == id);

                if (Book == null) return NotFound();

                await _dbBook.RemoveAsync(Book);
                await _dbBook.SaveAsync();

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

        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "EditBook")]
        public async Task<ActionResult<APIResponse>> UpdateBook(int id, [FromBody] BookDTOUpdate updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id) return BadRequest();

                Book Book = _mapper.Map<Book>(updateDTO);

                await _dbBook.UpdateAsync(Book);
                await _dbBook.SaveAsync();

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

        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "EditBookPartial")]
        public async Task<ActionResult<APIResponse>> UpdateBookPartial(int id, JsonPatchDocument<BookDTOUpdate> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0) return BadRequest();

                var Book = await _dbBook.GetAsync(BookDTO => BookDTO.Id == id, track: false);

                BookDTOUpdate updateDTO = _mapper.Map<BookDTOUpdate>(Book);

                if (Book == null) return NotFound();

                patchDTO.ApplyTo(updateDTO, ModelState);

                Book model = _mapper.Map<Book>(updateDTO);

                await _dbBook.UpdateAsync(model);
                await _dbBook.SaveAsync();

                if (!ModelState.IsValid) return BadRequest(ModelState);

                _response.Result = _mapper.Map<BookDTO>(Book);
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
