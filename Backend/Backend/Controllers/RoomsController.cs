using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using Backend.Core.IConfiguration;
using Backend.Dtos;
using Backend.Data;
using Backend.Requests;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResult<Room>>> GetRooms([FromQuery] PagingQuery query)
    {
        if (!int.TryParse(query.Limit, out int limitInt)
            || !int.TryParse(query.Offset, out int offsetInt))
        {
            return NotFound();
        }

        var rooms = await _unitOfWork.Rooms.GetAll();

        return await ApiResult<Room>.CreateAsync(
            rooms.ToList(),
            offsetInt,
            limitInt,
            "/rooms"
        );
    }

    [HttpGet("hotel/{id}")]
    public async Task<ActionResult<ApiResult<RoomDto>>> GetAvailableRoomsInHotel(string id,
        [FromQuery] PagingQuery query)
    {
        var hotelExists = await _unitOfWork.Hotels.Exists(id);

        if (!hotelExists)
        {
            return NotFound("Hotel with given id does not exist");
        }

        if (!int.TryParse(query.Limit, out int limitInt)
            || !int.TryParse(query.Offset, out int offsetInt))
        {
            return NotFound();
        }

        IEnumerable<Room> rooms = _unitOfWork.Rooms.GetAvailableRoomsById(id);

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
        var roomExists = await _unitOfWork.Rooms.Exists(id);
        if (!roomExists)
        {
            return NotFound();
        }

        var room = _unitOfWork.Rooms.GetById(id);
        var roomDto = _mapper.Map<RoomDto>(room);

        return roomDto;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutRoom(string id, RoomRequest roomRequest)
    {
        if (id != roomRequest.Id)
        {
            return BadRequest();
        }

        var hotelExists = await _unitOfWork.Hotels.Exists(roomRequest.HotelId);

        if (!hotelExists)
        {
            return NotFound("Hotel not found");
        }

        var room = _mapper.Map<Room>(roomRequest);

        if (!await RoomExists(id))
        {
            return NotFound();
        }

        try
        {
            _unitOfWork.Rooms.Update(room);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Room>> PostRoom(RoomRequest roomRequest)
    {
        var hotelExists = await _unitOfWork.Hotels.Exists(roomRequest.HotelId);

        if (!hotelExists)
        {
            return NotFound("Hotel not found");
        }

        var room = _mapper.Map<Room>(roomRequest);
        await _unitOfWork.Rooms.Add(room);
        var roomResponse = _mapper.Map<RoomRequest>(room);

        return CreatedAtAction(nameof(GetRoom), roomResponse);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteRoom(string id)
    {
        var roomExists = await _unitOfWork.Rooms.Exists(id);

        if (!roomExists)
        {
            return NotFound();
        }

        var room = await _unitOfWork.Rooms.GetById(id);

        _unitOfWork.Rooms.Delete(room);

        return NoContent();
    }

    private async Task<bool> RoomExists(string id)
    {
        return await _unitOfWork.Rooms.Exists(id);
    }
}