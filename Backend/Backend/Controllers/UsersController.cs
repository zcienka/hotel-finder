using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Backend.Data;
using Backend.Dtos;
using Backend.Interfaces;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Email)
            {
                return BadRequest();
            }

            try
            {
                await _userRepository.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                await _userRepository.Add(user);
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Email }, user);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.Delete(user);

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _userRepository.Exists(id);
        }

        // [HttpGet("user/{userEmail}")]
        // [Authorize]
        // public async Task<ActionResult<ApiResult<ReservationDto>>> GetUserReservations([FromQuery] PagingQuery query,
        //     string userEmail)
        // {
        //     if (!int.TryParse(query.Limit, out int limitInt)
        //         || !int.TryParse(query.Offset, out int offsetInt))
        //     {
        //         return NotFound();
        //     }
        //
        //
        //     var reservationDtos = reservations.Select(r =>
        //     {
        //         var reservationDto = _mapper.Map<ReservationDto>(r);
        //         reservationDto.Hotel = r.Hotel;
        //         return reservationDto;
        //     }).ToList();
        //
        //     return Ok(await ApiResult<ReservationDto>.CreateAsync(
        //         reservationDtos,
        //         offsetInt,
        //         limitInt,
        //         "/user"
        //     ));
        // }
    }
}