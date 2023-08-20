using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using Backend.Core.IConfiguration;
using Backend.Dtos;
using Backend.Data;

namespace Backend.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork; 

    public CommentsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResult<CommentDto>>> GetComments([FromQuery] PagingQuery query)
    {
        if (!int.TryParse(query.Limit, out int limitInt)
            || !int.TryParse(query.Offset, out int offsetInt))
        {
            return NotFound();
        }

        var comments = await _unitOfWork.Comments.GetAll();
        var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment));

        return await ApiResult<CommentDto>.CreateAsync(
            commentDtos.ToList(),
            offsetInt,
            limitInt,
            "/comments"
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(string id)
    {
        var commentExists = await _unitOfWork.Comments.Exists(id);

        if (!commentExists)
        {
            return NotFound("No comment with a given id found.");
        }

        var comment = _unitOfWork.Comments.GetById(id);
        var commentDto = _mapper.Map<CommentDto>(comment);

        return commentDto;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(string id, CommentDto commentDto)
    {
        var hotelExists = await _unitOfWork.Hotels.Exists(commentDto.HotelId);

        if (id != commentDto.Id)
        {
            return BadRequest();
        }

        if (!hotelExists)
        {
            return NotFound("Hotel not found");
        }

        var userExists = await _unitOfWork.Users.Exists(commentDto.UserEmail);

        if (!userExists)
        {
            return NotFound("User not found");
        }

        if (!await CommentExists(id))
        {
            return NotFound();
        }

        var comment = _mapper.Map<Comment>(commentDto);

        try
        {
            _unitOfWork.Comments.Update(comment);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> PostComment(CommentDto commentDto)
    {
        var hotelExists = await _unitOfWork.Hotels.Exists(commentDto.HotelId);

        if (!hotelExists)
        {
            return NotFound("Hotel not found");
        }

        var userExists = await _unitOfWork.Users.Exists(commentDto.UserEmail);

        if (!userExists)
        {
            return NotFound("User not found");
        }

        var comment = _mapper.Map<Comment>(commentDto);

        await _unitOfWork.Comments.Add(comment);

        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, commentDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(string id)
    {
        var comment = await _unitOfWork.Comments.GetById(id);

        if (comment == null)
        {
            return NotFound();
        }

        _unitOfWork.Comments.Delete(comment);

        return NoContent();
    }

    private Task<bool> CommentExists(string id)
    {
        return _unitOfWork.Comments.Exists(id);
    }
}
