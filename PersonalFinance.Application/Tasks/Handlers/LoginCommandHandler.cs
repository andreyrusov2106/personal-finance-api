using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Domain;
using System.Security.Principal;


namespace PersonalFinance.Application.Tasks.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        private readonly IPasswordHasher<User> _passwordHasher;


        public LoginCommandHandler(ITokenService tokenService,
                                   IUserRepository userRepository,
                                   IPasswordHasher<User> passwordHasher)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetUserAsync(request.LoginRequestDto.login);

            if (user == null)
                throw new UserNotFoundException(request.LoginRequestDto.login);

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.LoginRequestDto.password);

            if(result != PasswordVerificationResult.Success)
                throw new InvalidCredentionalsException();


            return _tokenService.CreateToken(user.Id.ToString(), user.Login, user.Name);

        }
    }
}
