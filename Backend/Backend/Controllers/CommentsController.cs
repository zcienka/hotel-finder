using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using Backend.Dtos;
using Backend.Data;
using Bogus.DataSets;
using System.Drawing.Drawing2D;
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

        // [HttpGet("hotel/{hotelId}")]
        // public async Task<ActionResult<ApiResult<CommentDto>>> GetCommentsByHotel(string hotelId, [FromQuery] PagingQuery query)
        // {
        //     if (!int.TryParse(query.Limit, out int limitInt)
        //         || !int.TryParse(query.Offset, out int offsetInt))
        //     {
        //         return NotFound();
        //     }
        //
        //     var comments = _context.Comments.Where(q => q.HotelId == hotelId).ToList();
        //     var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment)).ToList();
        //
        //     return Ok(await ApiResult<CommentDto>.CreateAsync(
        //         commentDtos,
        //         offsetInt,
        //         limitInt,
        //         "/comments/hotel"
        //     ));
        // }
        //
        // [HttpGet("user/{userEmail}")]
        // public async Task<ActionResult<ApiResult<CommentDto>>> GetCommentsByUser(string userEmail, [FromQuery] PagingQuery query)
        // {
        //     if (!int.TryParse(query.Limit, out int limitInt)
        //         || !int.TryParse(query.Offset, out int offsetInt))
        //     {
        //         return NotFound();
        //     }
        //
        //     var comments = _context.Comments.Where(q => q.UserEmail == userEmail).ToList();
        //
        //     var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment)).ToList();
        //
        //     return Ok(await ApiResult<CommentDto>.CreateAsync(
        //         commentDtos,
        //         offsetInt,
        //         limitInt,
        //         "/comments/user"
        //     ));
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(string id, Comment comment)
        {
            // var hotel = _context.Hotels.FirstOrDefault(h => h.Id.Equals(comment.HotelId));

            // if (hotel == null)
            // {
            //     return NotFound("Hotel not found");
            // }

            if (!id.Equals(comment.Id))
            {
                return BadRequest();
            }


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
            // var hotel = _context.Hotels.FirstOrDefault(h => commentDto.HotelId.Equals(h.Id));

            // if (hotel == null)
            // {
            //     return NotFound("Hotel not found");
            // }

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