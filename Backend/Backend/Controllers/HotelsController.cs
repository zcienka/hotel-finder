using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Backend.Requests;
using Backend.Data;
using Backend.Dtos;
using Backend.Interfaces;
using Backend.Responses;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HotelsController(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult<HotelResponse>>> GetHotels(
            [FromQuery] PagingQuery query,
            [FromQuery] string? name,
            [FromQuery] string? city,
            [FromQuery] DateTimeOffset? checkInDate,
            [FromQuery] DateTimeOffset? checkOutDate,
            [FromQuery] int? roomCount,
            [FromQuery] string? category)
        {   
            if (!int.TryParse(query.Limit, out int limitInt) ||
                !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            var hotels = _hotelRepository.GetSearchResults(
                name, city, checkInDate, checkOutDate, roomCount, category);

            var searchResponse = hotels.Select(hotel => _mapper.Map<HotelResponse>(hotel)).ToList();

            return Ok(await ApiResult<HotelResponse>.CreateAsync(
                searchResponse,
                offsetInt,
                limitInt,
                "/hotels"
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelResponse>> GetHotel(string id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);

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

            try
            {
                await _hotelRepository.Update(hotel);
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

            await _hotelRepository.Add(hotel);

            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotelRequest);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteHotel(string id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);

            if (hotel == null)
            {
                return NotFound("Hotel with a given id does not exist.");
            }

            await _hotelRepository.Delete(hotel);

            return NoContent();
        }

        private bool HotelExists(string id)
        {
            return _hotelRepository.Exists(id);
        }


        [HttpGet("{hotelId}/comments")]
        public async Task<ActionResult<ApiResult<CommentDto>>> GetCommentsByHotel(string hotelId, [FromQuery] PagingQuery query)
        {
            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            var comments = _hotelRepository.GetComments(hotelId);
            var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment)).ToList();

            return Ok(await ApiResult<CommentDto>.CreateAsync(
                commentDtos,
                offsetInt,
                limitInt,
                "/comments/hotel"
            ));
        }
    }
}