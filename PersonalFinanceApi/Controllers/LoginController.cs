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
    }
}
