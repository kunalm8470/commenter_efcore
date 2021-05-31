using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Core.Entities;
using Core.Services;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserService service,
            IPasswordService passwordService,
            IMapper mapper
        )
        {
            _service = service;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<UserResponseDto>> RegisterUserAsync([FromBody] RegisterUserDto dto)
        {
            try
            {
                User created = await _service.RegisterUserAsync(_mapper.Map<RegisterUserDto, User>(dto));
                return Ok(_mapper.Map<User, UserResponseDto>(created));
            }
            catch (UniqueConstraintException)
            {
                return Conflict(new HttpError("ENTRY_ALREADY_EXISTS", "Username / email already exists!"));
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserResponseDto>> LoginUserAsync([FromBody] LoginUserDto dto)
        {
            User found = await _service.FetchUserAsync(dto.Login);
            if (found == default)
            {
                return NotFound(new HttpError("ENTRY_NOT_FOUND", "No user found associated with the login"));
            }

            if (!_passwordService.VerifyPassword(dto.Password, found.Password))
            {
                return Unauthorized(new HttpError("INVALID_CREDENTIALS", "Incorrect password!"));
            }

            return Ok(_mapper.Map<User, UserResponseDto>(found));
        }
    }
}
