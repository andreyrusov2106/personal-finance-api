using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace PersonalFinance.Application.Tasks.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IUserRepository userRepository, 
            IPasswordHasher<User> passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher= passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string login = request.RegisterRequestDto.Login;
            string password = request.RegisterRequestDto.Password;
            string name = request.RegisterRequestDto.Name;

            var user = await _userRepository.GetUserAsync(login);

            if(user is not null)
            {
                throw new LoginIsNotUniqueException(login);
            }

            
            var newUser = new User(login, name);
            var passwordHash = _passwordHasher.HashPassword(newUser, password);
            newUser.ChangePassword(passwordHash);
            await _userRepository.AddUserAsync(newUser);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newUser.Id;

        }
    }
}
