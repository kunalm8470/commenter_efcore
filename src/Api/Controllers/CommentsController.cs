using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Core.Entities;
using Core.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _service;
        private readonly IMapper _mapper;
        private readonly PaginationService _paginationService;
        public CommentsController(
            ICommentService service,
            IMapper mapper,
            PaginationService paginationService
        )
        {
            _service = service;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<CommentResponseDto>>> GetComments([FromQuery] CommentSearchParams searchParams)
        {
            PageModel<Comment> comments = await _service.FetchCommentsAsync(searchParams);
            PageModel<CommentResponseDto> mappedComments = new(comments.Data.Select(_mapper.Map<Comment, CommentResponseDto>), comments.TotalCount);

            PagedResponse<CommentResponseDto> commentsResponse = _paginationService.BuildPagedResponse(mappedComments, searchParams.Page.Value, searchParams.Limit.Value);

            Response.Headers.Add("X-Pagination-Per-Page", searchParams.Limit.ToString());
            Response.Headers.Add("X-Pagination-Current-Page", searchParams.Page.ToString());
            Response.Headers.Add("X-Pagination-Total-Pages", commentsResponse.Pages.ToString());
            Response.Headers.Add("X-Pagination-Total-Entries", commentsResponse.TotalCount.ToString());

            return Ok(commentsResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResponseDto>> GetCommentById([FromRoute] int id)
        {
            Comment found = await _service.FetchCommentByIdAsync(id);
            if (found == default)
            {
                return NotFound(new HttpError("ENTRY_NOT_FOUND", "No comment found with given id"));
            }

            return Ok(_mapper.Map<Comment, CommentResponseDto>(found));
        }

        [HttpGet("Search")]
        public async Task<ActionResult<CommentResponseDto>> GetComment([FromQuery] CommentSearchParams searchParams)
        {
            Comment found = await _service.FetchCommentAsync(searchParams);
            if (found == default)
            {
                return NotFound(new HttpError("ENTRY_NOT_FOUND", "No post found with following search terms"));
            }

            return Ok(_mapper.Map<Comment, CommentResponseDto>(found));
        }

        [HttpPost]
        public async Task<ActionResult<CommentResponseDto>> AddPost([FromBody] AddCommentDto dto)
        {
            Comment c = _mapper.Map<AddCommentDto, Comment>(dto);
            Comment created = await _service.AddCommentAsync(c);
            return CreatedAtAction(nameof(GetCommentById), new { id = created.Id }, _mapper.Map<Comment, CommentResponseDto>(created));
        }

        [HttpPut]
        public async Task<ActionResult<CommentResponseDto>> UpdateComment([FromBody] UpdateCommentDto dto)
        {
            try
            {
                Comment c = _mapper.Map<UpdateCommentDto, Comment>(dto);
                Comment updated = await _service.UpdateCommentAsync(c);
                return Ok(_mapper.Map<Comment, CommentResponseDto>(updated));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HttpError("UPDATE_FAILED", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            try
            {
                await _service.DeleteCommentAsync(id);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HttpError("UPDATE_FAILED", ex.Message));
            }
        }
    }
}
