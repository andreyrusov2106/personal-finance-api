using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Tasks.Queries;
using PersonalFinance.Application.Tasks.Commands;

namespace PersonalFinanceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/accounts
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AccountItemDto>>> GetAll()
        {
            var query = new GetAllAccountsQuery();
            var tasks = await _mediator.Send(query);
            return Ok(tasks);
        }

        // GET: api/account(id)
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountItemDto>> GetAccount(Guid id)
        {
            var query = new GetAccountQuery(id);
            var account = await _mediator.Send(query);

            return Ok(account);
        }

        // PUT: api/accounts(id)
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AccountItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AccountItemDto>> CloseAccount(Guid id)
        {

            var command = new CloseAccountCommand(id);
            var account = await _mediator.Send(command);
            return Ok(account);
        }

        // POST: api/account
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(AccountItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountItemDto>> CreateAccount(
            [FromBody] CreateAccountDto accountDto)
        {
            var command = new CreateAccountCommand(accountDto);
            var createdAccount = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetAccount),
                new { id = createdAccount.Id },
                createdAccount);
        }

        // DELETE: api/accounts/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAccount(Guid id)
        {
            var command = new DeleteAccountCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }


        // POST /api/accounts/{id}/deposit
        [HttpPost("{id}/deposit")]
        [ProducesResponseType(typeof(AccountItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountItemDto>> DepositAccount(Guid id,
            [FromBody] DepositAccountDto depositAccountDto)
        {
            var command = new DepositCommand(id,depositAccountDto.Amount);
            var account = await _mediator.Send(command);

            return Ok(account);
        }

        // POST /api/accounts/{id}/withdraw
        [HttpPost("{id}/withdraw")]
        [ProducesResponseType(typeof(AccountItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountItemDto>> WithdrawAccount(Guid id,
            [FromBody] WithdrawAccountDto withdrawAccountDto)
        {
            var command = new WithdrawCommand(id, withdrawAccountDto.Amount);
            var account = await _mediator.Send(command);

            return Ok(account);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
        {
            return Ok("You are admin");
        }

        [Authorize]
        [HttpGet("/api/auth/me")]
        public IActionResult Me()
        {
            var claims = HttpContext.User.Claims;
            return Ok( new {
                id = claims.First(claim => claim.Type.Equals(System.Security.Claims.ClaimTypes.NameIdentifier)).Value,
                role = claims.First(claim=>claim.Type.Equals(System.Security.Claims.ClaimTypes.Role)).Value,
                name = claims.First(claim => claim.Type.Equals(System.Security.Claims.ClaimTypes.Name)).Value

            });
        }



    }
}
