using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using Backend.Dtos;
using Backend.Data;
using Backend.Interfaces;
using Bogus.DataSets;
using System.Drawing.Drawing2D;
using Backend.Requests;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomsController(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
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

            var rooms = await _roomRepository.GetAll();

            return await ApiResult<Room>.CreateAsync(
                rooms.ToList(),
                offsetInt,
                limitInt,
                "/rooms"
            );
        }

        [HttpGet("hotel/{id}")]
        public async Task<ActionResult<ApiResult<RoomDto>>> GetAvailableRoomsInHotel(string id,
            [FromQuery] PagingQuery query = null)
        {
            var hotelExists = _roomRepository.HotelExists(id);

            if (!hotelExists)
            {
                return NotFound("Hotel with given id does not exist");
            }

            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            List<Room> rooms = _roomRepository.GetAvailableRoomsById(id);

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
            var room = await _roomRepository.GetByIdAsync(id);

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

            var hotelExists = _roomRepository.HotelExists(id);

            if (!hotelExists)
            {
                return NotFound("Hotel not found");
            }

            try
            {
                await _roomRepository.Update(room);
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
        public async Task<ActionResult<Room>> PostRoom(RoomRequest roomRequest)
        {
            var hotelExists = _roomRepository.HotelExists(roomRequest.HotelId);

            if (!hotelExists)
            {
                return NotFound("Hotel not found");
            }

            var room = _mapper.Map<Room>(roomRequest);
            await _roomRepository.Add(room);
            var roomResponse = _mapper.Map<RoomRequest>(room);

            return CreatedAtAction(nameof(GetRoom), roomResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            await _roomRepository.Delete(room);

            return NoContent();
        }

        private bool RoomExists(string id)
        {
            return _roomRepository.Exists(id);
        }
    }
}