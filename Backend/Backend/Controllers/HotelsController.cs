using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<ApiResult<HotelResponse>>> GetHotels([FromQuery] PagingQuery query,
            [FromQuery] string city = null)
        {
            if (_context.Hotels.ToList().Count == 0)
            {
                return NotFound("No hotels found");
            }

            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            var hotels = await _context.Hotels.ToListAsync();

            if (string.IsNullOrEmpty(city))
            {
                var hotelResponses = hotels.Select(hotel => _mapper.Map<HotelResponse>(hotel)).ToList();

                return Ok(await ApiResult<HotelResponse>.CreateAsync(
                    hotelResponses,
                    offsetInt,
                    limitInt,
                    "/hotels"
                ));
            }
            else
            {
                var searchResult = hotels.Where(x => x.City.ToLower().Contains(city.ToLower())).ToList();
                var hotelResponses = searchResult.Select(hotel => _mapper.Map<HotelResponse>(hotel)).ToList();

                return Ok(await ApiResult<HotelResponse>.CreateAsync(
                    hotelResponses,
                    offsetInt,
                    limitInt,
                    "/hotels"
                ));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelResponse>> GetHotel(string id)
        {
            if (_context.Hotels.ToList().Count == 0)
            {
                return NotFound("No hotels found");
            }

            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound("No hotel with a given id found");
            }

            var hotelResponse = _mapper.Map<HotelResponse>(hotel);

            return hotelResponse;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutHotel(string id, Hotel hotel)
        {
            if (id.Equals(hotel.Id))
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
        // [Authorize]
        public async Task<IActionResult> PostHotel(HotelRequest hotelRequest)
        {
            var hotel = _mapper.Map<Hotel>(hotelRequest);

            _context.Hotels.Add(hotel);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteHotel(string id)
        {
            var hotel = _context.Hotels.FirstOrDefault(hotel => hotel.Id == id);

            if (hotel == null)
            {
                return NotFound("Hotel with a given id does not exist.");
            }

            var comments = _context.Comments.Where(comment => comment.HotelId == id);
            
            if (comments.Any())
            {
                _context.Comments.RemoveRange(comments);
            }

            var reservations = _context.Reservations.Where(reservation => reservation.HotelId == id);

            if (reservations.Any())
            {
                _context.Reservations.RemoveRange(reservations);
            }

            var rooms = _context.Rooms.Where(room => room.HotelId == id);

            if (rooms.Any())
            {
                _context.Rooms.RemoveRange(rooms);
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        private bool HotelExists(string id)
        {
            return (_context.Hotels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}