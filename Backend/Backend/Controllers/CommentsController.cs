using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using System.Xml;
using System.Xml.Linq;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult<CommentDto>>> GetComments([FromQuery] PagingQuery query)
        {
            if (_context.Comments.ToList().Count == 0)
            {
                return NotFound("No comments found");
            }

            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            var comments = await _context.Comments.ToListAsync();
            var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment)).ToList();

            return Ok(await ApiResult<CommentDto>.CreateAsync(
                commentDtos,
                offsetInt,
                limitInt,
                "/comments"
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            if (_context.Comments.ToList().Count == 0)
            {
                return NotFound("No comments found");
            }

            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound("No comment with a given id found.");
            }

            return comment;
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<ApiResult<CommentDto>>> GetCommentsByHotel(int hotelId, [FromQuery] PagingQuery query)
        {
            if (!int.TryParse(query.Limit, out int limitInt)
                || !int.TryParse(query.Offset, out int offsetInt))
            {
                return NotFound();
            }

            var comments = _context.Comments.Where(q => q.HotelId == hotelId).ToList();

            var commentDtos = comments.Select(comment => _mapper.Map<CommentDto>(comment)).ToList();

            return Ok(await ApiResult<CommentDto>.CreateAsync(
                commentDtos,
                offsetInt,
                limitInt,
                "/comments/hotel"
            ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == comment.HotelId);

            if (hotel == null)
            {
                return NotFound("Hotel not found");
            }

            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == commentDto.HotelId);

            if (hotel == null)
            {
                return NotFound("Hotel not found");
            }

            var comment = _mapper.Map<Comment>(commentDto);

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, commentDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = _context.Comments.FirstOrDefault(comment => comment.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}