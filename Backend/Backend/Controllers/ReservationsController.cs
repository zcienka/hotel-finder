using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Backend.Dtos;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReservationsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResult<ReservationDto>>> GetReservations([FromQuery] PagingQuery query)
        {
            if (_context.Reservations.ToList().Count == 0)
            {
                return NotFound("No reservations found");
            }

            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            var reservations = await _context.Reservations.ToListAsync();
            var reservationsDtos = reservations.Select(reservation => _mapper.Map<ReservationDto>(reservation)).ToList();

            return Ok(await ApiResult<ReservationDto>.CreateAsync(
                reservationsDtos,
                offsetInt,
                limitInt,
                "/reservations"
            ));
        }

        [HttpGet("user/{userEmail}")]
        [Authorize]
        public async Task<ActionResult<ApiResult<ReservationDto>>> GetUserReservations([FromQuery] PagingQuery query, string userEmail)
        {
            if (_context.Reservations.ToList().Count == 0)
            {
                return NotFound("No reservations found");
            }

            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {   
                return NotFound();
            }

            var reservations = _context.Reservations.Where(r => r.UserEmail == userEmail)
                .Include(r => r.Hotel).ToList();
            var reservationDtos = reservations.Select(r =>
            {
                var reservationDto = _mapper.Map<ReservationDto>(r);
                reservationDto.Hotel = r.Hotel;
                return reservationDto;
            }).ToList();

            return Ok(await ApiResult<ReservationDto>.CreateAsync(
                reservationDtos,
                offsetInt,
                limitInt,
                "/user"
            ));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Reservation>> GetReservation(string id)
        {
            if (_context.Reservations.ToList().Count == 0)
            {
                return NotFound("No reservations found");
            }

            var reservation = await _context.Reservations.FindAsync(id);

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

            var hotel = _context.Hotels.FirstOrDefault(h => h.Id.Equals(reservation.RoomId));

            if (hotel == null)
            {
                return NotFound("Hotel not found");
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
        [Authorize]
        public async Task<ActionResult<Reservation>> PostReservation(ReservationDto reservationDto)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == reservationDto.HotelId);

            if (hotel == null)
            {
                return NotFound("Hotel not found");
            }

            var room = _context.Rooms.FirstOrDefault(r => r.HotelId == reservationDto.HotelId && r.Id == reservationDto.RoomId);

            if (room == null || room.HotelId != reservationDto.HotelId)
            {
                return NotFound("Room with that id not found");
            }

            if (reservationDto.CheckOutDate <= reservationDto.CheckInDate)
{
    return BadRequest("Check-out date must be later than the check-in date. Please select a valid check-out date.");
}

            var reservations =  _context.Reservations.ToList();

            bool isReservationConflict = reservations.Any(r =>
                r.RoomId == reservationDto.RoomId && r.HotelId == reservationDto.HotelId &&
                !(reservationDto.CheckOutDate <= r.CheckInDate || reservationDto.CheckInDate >= r.CheckOutDate)
            );
            if (isReservationConflict)
            {
                return BadRequest("Reservation conflicts with existing reservations");
            }

            var reservation = _mapper.Map<Reservation>(reservationDto);
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReservation(string id)
        {
            if (_context.Reservations.ToList().Count == 0)
            {
                return NotFound("No reservations found");
            }

            var reservation = _context.Reservations.FirstOrDefault(reservation => reservation.Id.Equals(id));

            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(string id)
        {
            return (_context.Reservations?.Any(e => e.Id.Equals(id))).GetValueOrDefault();
        }
    }
}