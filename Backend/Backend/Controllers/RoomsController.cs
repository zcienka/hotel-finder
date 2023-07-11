using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using Backend.Dtos;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoomsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult<Room>>> GetRooms([FromQuery] PagingQuery query)
        {
            if (_context.Rooms.ToList().Count == 0)
            {
                return NotFound("No rooms found");
            }

            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            var rooms = _context.Rooms.ToList();

            return await ApiResult<Room>.CreateAsync(
                rooms,
                offsetInt,
                limitInt,
                "/rooms"
            );
        }

        [HttpGet("hotel/{id}")]
        public async Task<ActionResult<ApiResult<RoomDto>>> GetAvailableRoomsInHotel(string id,
            [FromQuery] PagingQuery query)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);

            if (hotel == null)
            {
                return NotFound("Hotel with given id does not exist");
            }

            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            if (_context.Rooms.ToList().Count == 0)
            {
                return NotFound("No rooms found");
            }

            var reservedRooms = _context.Reservations
                .Where(r => r.HotelId == id &&
                            r.CheckInDate <= DateTime.Now &&
                            r.CheckOutDate >= DateTime.Now)
                .Select(r => r.RoomId)
                .ToList();

            var rooms = _context.Rooms
                .Where(r => !reservedRooms.Contains(r.Id) &&
                            r.HotelId == id)
                .ToList();


            if (rooms.Count == 0)
            {
                return NotFound("No available rooms in the given hotel");
            }

            var roomDtos = rooms.Select(room => _mapper.Map<RoomDto>(room)).ToList();

            return await ApiResult<RoomDto>.CreateAsync(
                roomDtos,
                offsetInt,
                limitInt,
                "/rooms"
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoom(string id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            var roomDto = _mapper.Map<RoomDto>(room);

            return roomDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(string id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            var hotel = _context.Hotels.FirstOrDefault(h => h.Id.Equals(room.HotelId));

            if (hotel == null)
            {
                return NotFound("Hotel not found");
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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
        public async Task<ActionResult<Room>> PostRoom(RoomDto roomDto)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id.Equals(roomDto.HotelId));

            if (hotel == null)
            {
                return NotFound("Hotel not found");
            }

            var room = _mapper.Map<Room>(roomDto);
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            var room = _context.Rooms.FirstOrDefault(room => room.Id.Equals(id));

            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);

            var reservation = _context.Reservations.FirstOrDefault(h => h.RoomId.Equals(id));

            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(string id)
        {
            return (_context.Rooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}