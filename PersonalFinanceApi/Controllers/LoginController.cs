using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Tasks.Commands;

namespace PersonalFinanceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController: ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // POST /api/auth/login
        [HttpPost("/api/auth/login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequestDto loginRequestDto)

        {
            var command = new LoginCommand(loginRequestDto);
            var token = await _mediator.Send(command);

            return Ok(token);
        }


        // POST /api/auth/register
        [HttpPost("/api/auth/register")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Guid>> Register([FromBody] RegisterRequestDto registerRequestDto)

        {
            var command = new RegisterCommand(registerRequestDto);
            var userId = await _mediator.Send(command);

            return Created("", userId);
        }
    }
}
