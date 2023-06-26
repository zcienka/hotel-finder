using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

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

            var rooms =  _context.Rooms.ToList();

            return await ApiResult<Room>.CreateAsync(
                rooms,
                offsetInt,
                limitInt,
                "/rooms"
            );
        }

        [HttpGet("hotel/{id}")]
        public async Task<ActionResult<List<Room>>> GetRoomsInHotel(int id)
        {   
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);

            if (hotel == null)
            {
                return NotFound("Hotel with given id does not exist");
            }

            if (_context.Rooms.ToList().Count == 0)
            {
                return NotFound("No rooms found");
            }

            var rooms = _context.Rooms
                .Where(r => r.HotelId == id)
                .ToList();
            
            if (rooms.Count == 0)
            {
                return NotFound("No rooms registered in a given hotel");
            }

            return Ok(rooms.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == room.HotelId);

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
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == roomDto.HotelId);

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
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}