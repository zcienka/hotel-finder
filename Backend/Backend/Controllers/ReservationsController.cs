using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Backend.Dtos;
using Backend.Requests;
using Backend.Data;
using Backend.Interfaces;
using Bogus.DataSets;
using System.Drawing.Drawing2D;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResult<ReservationDto>>> GetReservations([FromQuery] PagingQuery query)
        {
            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            var reservations = await _reservationRepository.GetAll();
            var reservationsDtos =
                reservations.Select(reservation => _mapper.Map<ReservationDto>(reservation)).ToList();

            return Ok(await ApiResult<ReservationDto>.CreateAsync(
                reservationsDtos,
                offsetInt,
                limitInt,
                "/reservations"
            ));
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Reservation>> GetReservation(string id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutReservation(string id, Reservation reservation)
        {
            if (!id.Equals(reservation.Id))
            {
                return BadRequest();
            }

            var hotelExists = _reservationRepository.HotelExists(reservation.HotelId);

            if (!hotelExists)
            {
                return NotFound("Hotel not found");
            }

            try
            {
                await _reservationRepository.Update(reservation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        // [Authorize]
        public async Task<ActionResult<Reservation>> PostReservation(ReservationRequest reservationRequest)
        {
            var hotelExists = _reservationRepository.HotelExists(reservationRequest.HotelId);

            if (!hotelExists)
            {
                return NotFound("Hotel not found");
            }

            var room = _reservationRepository.FindRoomInHotel(reservationRequest.HotelId, reservationRequest.RoomId);

            if (room == null)
            {
                return NotFound("Room with that id not found");
            }

            if (reservationRequest.CheckOutDate <= reservationRequest.CheckInDate)
            {
                return BadRequest(
                    "Check-out date must be later than the check-in date.");
            }

            var reservations = await _reservationRepository.GetAll();

            bool isReservationConflict = reservations.Any(r =>
                r.RoomId == reservationRequest.RoomId && r.HotelId == reservationRequest.HotelId &&
                !(reservationRequest.CheckOutDate <= r.CheckInDate || reservationRequest.CheckInDate >= r.CheckOutDate)
            );
            if (isReservationConflict)
            {
                return BadRequest("Reservation conflicts with existing reservations");
            }

            var reservation = _mapper.Map<Reservation>(reservationRequest);
            await _reservationRepository.Add(reservation);

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservationRequest);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReservation(string id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            await _reservationRepository.Delete(reservation);

            return NoContent();
        }

        private bool ReservationExists(string id)
        {
            return _reservationRepository.Exists(id);
        }
    }
}