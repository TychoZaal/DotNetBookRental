using AutoMapper;
using BookRental.NET.Models.Dto;
using BookRental.NET.Models;
using BookRental.NET.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookRental.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IReservationRepository _dbReservation;
        private readonly IMapper _mapper;

        public ReservationController(IReservationRepository dbReservation, IMapper mapper)
        {
            _dbReservation = dbReservation;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReservationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ReservationDTO>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReservations()
        {
            try
            {
                IEnumerable<Reservation> ReservationList = await _dbReservation.GetAllAsync();
                _response.Result = _mapper.Map<List<ReservationDTO>>(ReservationList);
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

        [HttpGet("{id:int}", Name = "GetReservation")]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReservation(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var Reservation = await _dbReservation.GetAsync(u => u.Id == id);

                if (Reservation == null) return NotFound();

                _response.Result = _mapper.Map<ReservationDTO>(Reservation);
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
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateReservation([FromBody] ReservationDTOCreate createDTO)
        {
            try
            {
                if (createDTO == null) return BadRequest();

                Reservation Reservation = _mapper.Map<Reservation>(createDTO);

                await _dbReservation.CreateAsync(Reservation);
                await _dbReservation.SaveAsync();

                _response.Result = _mapper.Map<ReservationDTO>(Reservation);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetReservation", new { id = Reservation.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteReservation")]
        public async Task<ActionResult<APIResponse>> DeleteReservation(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var Reservation = await _dbReservation.GetAsync(Reservation => Reservation.Id == id);

                if (Reservation == null) return NotFound();

                await _dbReservation.RemoveAsync(Reservation);
                await _dbReservation.SaveAsync();

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

        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "EditReservation")]
        public async Task<ActionResult<APIResponse>> UpdateReservation(int id, [FromBody] ReservationDTOUpdate updateDTO, bool toApprove)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id) return BadRequest();

                Reservation Reservation = _mapper.Map<Reservation>(updateDTO);

                await _dbReservation.UpdateAsync(Reservation, toApprove);
                await _dbReservation.SaveAsync();

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
