using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

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

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResult<ReservationDto>>> GetUserReservations([FromQuery] PagingQuery query, int userId)
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

            var reservations = _context.Reservations.Where(r => r.UserId == userId).ToList();
            var reservationDtos = reservations.Select(r => _mapper.Map<ReservationDto>(r)).ToList();

            return Ok(await ApiResult<ReservationDto>.CreateAsync(
                reservationDtos,
                offsetInt,
                limitInt,
                "/user"
            ));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
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
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == reservation.RoomId);

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
        // [Authorize]
        public async Task<ActionResult<Reservation>> PostReservation(ReservationDto reservationDto)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == reservationDto.HotelId);

            if (hotel == null)
            {
                return NotFound("Hotel not found");
            }

            var room = _context.Rooms.FirstOrDefault(h => h.HotelId == reservationDto.HotelId);

            if (room == null || room.HotelId != reservationDto.HotelId)
            {
                return NotFound("Room not found");
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

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            if (_context.Reservations.ToList().Count == 0)
            {
                return NotFound("No reservations found");
            }

            var reservation = _context.Reservations.FirstOrDefault(reservation => reservation.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}