using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HotelsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<HotelDto>>> GetHotels([FromQuery] string search = null)
        {
            if (_context.Hotels == null)
            {
                return NotFound("No hotels found");
            }

            if (string.IsNullOrEmpty(search))
            {
                var hotels = await _context.Hotels.ToListAsync();
                return Ok(hotels.Select(hotel => _mapper.Map<HotelDto>(hotel)));
            }
            else
            {
                var searchResult = _context.Hotels.Where(q => q.Name.Contains(search));
                return Ok(searchResult.Select(hotel => _mapper.Map<HotelDto>(hotel)));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            if (_context.Hotels == null)
            {
                return NotFound("No hotels found");
            }

            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound("No hotel with a given id found");
            }

            return hotel;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest("Hotel with a given id does not exist.");
            }

            _context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound("Hotel with a given id does not exist.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostHotel(HotelDto hotelDto)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hotels' is null.");
            }
            var hotel = _mapper.Map<Hotel>(hotelDto);

            _context.Hotels.Add(hotel);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound("Hotel with a given id does not exist.");
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HotelExists(int id)
        {
            return (_context.Hotels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}