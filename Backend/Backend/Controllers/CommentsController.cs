using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using Backend.Dtos;
using Backend.Data;
using Backend.Interfaces;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository; 

        public CommentsController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
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

            var comments = await _commentRepository.GetAll();
            var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment)).ToList();

            return Ok(await ApiResult<CommentDto>.CreateAsync(
                commentDtos,
                offsetInt,
                limitInt,
                "/comments"
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(string id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound("No comment with a given id found.");
            }

            return comment;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(string id, CommentDto commentDto)
        {
            var hotelExists = _commentRepository.HotelExists(commentDto.HotelId);

            if (!hotelExists)
            {
                return NotFound("Hotel not found");
            }

            if (!id.Equals(commentDto.Id))
            {
                return NotFound();
            }

            var userExists = _commentRepository.UserExists(commentDto.UserEmail);

            if (!userExists)
            {
                return NotFound("User not found");
            }

            var comment = _mapper.Map<Comment>(commentDto);

            try
            {
                await _commentRepository.Update(comment);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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
        public async Task<ActionResult<Comment>> PostComment(CommentDto commentDto)
        {
            var hotelExists = _commentRepository.HotelExists(commentDto.HotelId);

            if (!hotelExists)
            {
                return NotFound("Hotel not found");
            }

            var userExists = _commentRepository.UserExists(commentDto.UserEmail);

            if (!userExists)
            {
                return NotFound("User not found");
            }

            var comment = _mapper.Map<Comment>(commentDto);

            await _commentRepository.Add(comment);

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, commentDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            await _commentRepository.Delete(comment);

            return NoContent();
        }

        private bool CommentExists(string id)
        {
            return _commentRepository.Exists(id);
        }
    }
}