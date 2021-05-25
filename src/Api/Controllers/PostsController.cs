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
    public class PostsController : ControllerBase
    {
        private readonly IPostService _service;
        private readonly IMapper _mapper;
        private readonly PaginationService _paginationService;
        public PostsController(
            IPostService service,
            IMapper mapper,
            PaginationService paginationService
        )
        {
            _service = service;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<PostResponseDto>>> GetPosts([FromQuery] PostSearchParams searchParams)
        {
            PageModel<Post> posts = await _service.FetchPostsAsync(searchParams);
            PageModel<PostResponseDto> mappedPosts = new(posts.Data.Select(_mapper.Map<Post, PostResponseDto>), posts.TotalCount);

            PagedResponse<PostResponseDto> postsResponse = _paginationService.BuildPagedResponse(mappedPosts, searchParams.Page.Value, searchParams.Limit.Value);

            Response.Headers.Add("X-Pagination-Per-Page", searchParams.Limit.ToString());
            Response.Headers.Add("X-Pagination-Current-Page", searchParams.Page.ToString());
            Response.Headers.Add("X-Pagination-Total-Pages", postsResponse.Pages.ToString());
            Response.Headers.Add("X-Pagination-Total-Entries", postsResponse.TotalCount.ToString());

            return Ok(postsResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponseDto>> GetPostById([FromRoute] int id)
        {
            Post found = await _service.FetchPostByIdAsync(id);
            if (found == default)
            {
                return NotFound(new HttpError("ENTRY_NOT_FOUND", "No post found with given id"));
            }

            return Ok(_mapper.Map<Post, PostResponseDto>(found));
        }

        [HttpGet("Search")]
        public async Task<ActionResult<PostResponseDto>> GetPost([FromQuery] PostSearchParams searchParams)
        {
            Post found = await _service.FetchPostAsync(searchParams);
            if (found == default)
            {
                return NotFound(new HttpError("ENTRY_NOT_FOUND", "No post found with following search terms"));
            }

            return Ok(_mapper.Map<Post, PostResponseDto>(found));
        }

        [HttpPost]
        public async Task<ActionResult<PostResponseDto>> AddPost([FromBody] AddPostDto dto)
        {
            Post p = _mapper.Map<AddPostDto, Post>(dto);
            Post created = await _service.AddPostAsync(p);
            return CreatedAtAction(nameof(GetPostById), new { id = created.Id }, _mapper.Map<Post, PostResponseDto>(created));
        }

        [HttpPut]
        public async Task<ActionResult<PostResponseDto>> UpdatePost([FromBody] UpdatePostDto dto)
        {
            try
            {
                Post p = _mapper.Map<UpdatePostDto, Post>(dto);
                Post updated = await _service.UpdatePostAsync(p);
                return Ok(_mapper.Map<Post, PostResponseDto>(updated));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HttpError("UPDATE_FAILED", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            try
            {
                await _service.DeletePostAsync(id);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HttpError("UPDATE_FAILED", ex.Message));
            }
        }
    }
}
