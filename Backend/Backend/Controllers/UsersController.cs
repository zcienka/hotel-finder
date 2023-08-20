using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Backend.Core.IConfiguration;
using Backend.Data;
using Backend.Dtos;

namespace Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _unitOfWork.Users.GetAll();
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {
        var userExists = await _unitOfWork.Users.Exists(id);

        if (!userExists)
        {
            return NotFound();
        }

        var user = _unitOfWork.Users.GetById(id);
        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
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
            _unitOfWork.Users.Update(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await UserExists(id))
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
    public async Task<ActionResult<User>> PostUser(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);

        try
        {
            await _unitOfWork.Users.Add(user);
        }
        catch (DbUpdateException)
        {
            if (await UserExists(userDto.Email))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction(nameof(GetUser), new { Email = userDto.Email }, userDto);
    }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _unitOfWork.Users.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        _unitOfWork.Users.Delete(user);

        return NoContent();
    }

    private async Task<bool> UserExists(string id)
    {
        return await _unitOfWork.Users.Exists(id);
    }

    [HttpGet("{userId}/reservations")]
    [Authorize]
    public async Task<ActionResult<ApiResult<ReservationDto>>> GetUserReservations([FromQuery] PagingQuery query,
        string userId)
    {
        if (!int.TryParse(query.Limit, out int limitInt)
            || !int.TryParse(query.Offset, out int offsetInt))
        {
            return NotFound();
        }

        var reservations = _unitOfWork.Users.GetReservations(userId);

        var reservationDtos = reservations.Select(r => _mapper.Map<ReservationDto>(r)).ToList();

        return Ok(await ApiResult<ReservationDto>.CreateAsync(
            reservationDtos,
            offsetInt,
            limitInt,
            "/user"
        ));
    }


    [HttpGet("{userId}/comments")]
    public async Task<ActionResult<ApiResult<CommentDto>>> GetCommentsByUser(string userId,
        [FromQuery] PagingQuery query)
    {
        if (!int.TryParse(query.Limit, out int limitInt)
            || !int.TryParse(query.Offset, out int offsetInt))
        {
            return NotFound();
        }

        var comments = _unitOfWork.Users.GetComments(userId);

        var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment)).ToList();

        return Ok(await ApiResult<CommentDto>.CreateAsync(
            commentDtos,
            offsetInt,
            limitInt,
            "/comments"
        ));
    }
}