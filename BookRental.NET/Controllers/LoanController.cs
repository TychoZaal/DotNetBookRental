using AutoMapper;
using BookRental.NET.Models.Dto;
using BookRental.NET.Models;
using BookRental.NET.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BookRental.NET.Data;
using Microsoft.EntityFrameworkCore;

namespace BookRental.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILoanRepository _dbLoan;
        private readonly IMapper _mapper;
        private readonly BookRentalDbContext _dbContext;

        public LoanController(ILoanRepository dbLoan, IMapper mapper, BookRentalDbContext dbContext)
        {
            _dbLoan = dbLoan;
            _mapper = mapper;
            this._dbContext = dbContext;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LoanDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<LoanDTO>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetLoans()
        {
            try
            {
                IEnumerable<Loan> LoanList = await _dbLoan.GetAllAsync();
                _response.Result = _mapper.Map<List<LoanDTO>>(LoanList);
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

        [HttpGet("{id:int}", Name = "GetLoan")]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetLoan(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var Loan = await _dbLoan.GetAsync(u => u.Id == id);

                if (Loan == null) return NotFound();

                _response.Result = _mapper.Map<LoanDTO>(Loan);
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
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateLoan(int user_id, int book_id)
        {
            try
            {
                Book book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == book_id);
                if (book == null) return NotFound();

                User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user_id);
                if (user == null) return NotFound();

                LoanDTOCreate createDTO = new LoanDTOCreate();
                createDTO.StartDate = DateTime.Now;
                createDTO.User = user;
                createDTO.Book = book;
                createDTO.EndDate = null;

                Loan Loan = _mapper.Map<Loan>(createDTO);

                await _dbLoan.CreateAsync(Loan);
                await _dbLoan.SaveAsync();

                _response.Result = _mapper.Map<LoanDTO>(Loan);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetLoan", new { id = Loan.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteLoan")]
        public async Task<ActionResult<APIResponse>> DeleteLoan(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var Loan = await _dbLoan.GetAsync(Loan => Loan.Id == id);

                if (Loan == null) return NotFound();

                await _dbLoan.RemoveAsync(Loan);
                await _dbLoan.SaveAsync();

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

        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(LoanDTO), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "EditLoan")]
        public async Task<ActionResult<APIResponse>> EndLoanAsync(int id)
        {
            try
            {
                LoanDTOUpdate updateDTO = _mapper.Map<LoanDTOUpdate>(await _dbLoan.GetAsync(Loan => Loan.Id == id));

                if (updateDTO == null) return BadRequest();

                Loan Loan = _mapper.Map<Loan>(updateDTO);
                Loan.EndDate = DateTime.Now;

                await _dbLoan.EndLoanAsync(Loan);
                await _dbLoan.SaveAsync();

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
    }
}
