using MediatR;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Commands;


namespace PersonalFinance.Application.Tasks.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if(!(request.LoginRequestDto.login.Equals("andrey") 
                && request.LoginRequestDto.password.Equals("12345")))
            {
                throw new InvalidCredentionalsException();
            }
            return _tokenService.CreateToken("123", "User", "Andrey");

        }
    }
}
