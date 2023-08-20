using AutoMapper;
using Backend.Core.IConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Backend.Requests;
using Backend.Data;
using Backend.Dtos;
using Backend.Responses;

namespace Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HotelsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
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

        var hotels = _unitOfWork.Hotels.GetSearchResults(
            name, city, checkInDate, checkOutDate, roomCount, category);

        var searchResponse = hotels.Select(hotel => _mapper.Map<HotelResponse>(hotel)).ToList();

        return await ApiResult<HotelResponse>.CreateAsync(
            searchResponse,
            offsetInt,
            limitInt,
            "/hotels"
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelResponse>> GetHotel(string id)
    {
        var hotelExists = await HotelExists(id);

        if (!hotelExists)
        {
            return NotFound("No hotel with a given id found");
        }

        var hotel = _unitOfWork.Hotels.GetById(id);

        var hotelResponse = _mapper.Map<HotelResponse>(hotel);

        return hotelResponse;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutHotel(string id, HotelRequest hotelRequest)
    {
        var hotelExists = HotelExists(id);

        if (!await hotelExists)
        {
            return BadRequest("No hotel with a given id found");
        }

        var hotel = _mapper.Map<Hotel>(hotelRequest);

        try
        {
            _unitOfWork.Hotels.Update(hotel);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Hotel>> PostHotel(HotelRequest hotelRequest)
    {
        var hotel = _mapper.Map<Hotel>(hotelRequest);

        await _unitOfWork.Hotels.Add(hotel);

        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotelRequest);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteHotel(string id)
    {
        var hotelExists = await HotelExists(id);

        if (!hotelExists)
        {
            return NotFound("No hotel with a given id found");
        }
        var hotel = await _unitOfWork.Hotels.GetById(id);

        _unitOfWork.Hotels.Delete(hotel);

        return NoContent();
    }

    private Task<bool> HotelExists(string id)
    {
        return _unitOfWork.Hotels.Exists(id);
    }


    [HttpGet("{hotelId}/comments")]
    public async Task<ActionResult<ApiResult<CommentDto>>> GetCommentsByHotel(string hotelId,
        [FromQuery] PagingQuery query)
    {
        if (!int.TryParse(query.Limit, out int limitInt)
            || !int.TryParse(query.Offset, out int offsetInt))
        {
            return NotFound();
        }

        var comments = await _unitOfWork.Comments.GetAllByHotel(hotelId);
        var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment)).ToList();

        return Ok(await ApiResult<CommentDto>.CreateAsync(
            commentDtos,
            offsetInt,
            limitInt,
            "/hotel"
        ));
    }
}