using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Tasks.Handlers;
using PersonalFinance.Domain;
using PersonalFinance.Infrastructure;
using PersonalFinance.Infrastructure.Concurency;
using PersonalFinance.Infrastructure.Repositories;
using PersonalFinanceApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class LoginHandlerTests
    {
        [TestMethod]
        public async Task LoginCommandHandler_Should_Throw_UserNotFoundException()
        {
            // ARRANGE(Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<Program>();
            IConfiguration configuration = configurationBuilder.Build();

            var context = new AppDbContext(options);
            UserRepository userRepository = new UserRepository(context);
            TokenService tokenService = new TokenService(configuration);


            LoginCommandHandler loginCommandHandler = new LoginCommandHandler(tokenService, userRepository, passwordHasher);

            LoginRequestDto loginRequestDto = new LoginRequestDto("testuser", "Secret123!");

            LoginCommand command = new LoginCommand(loginRequestDto);


            // ACT (Действие)

            // ASSERT (Проверка)
            await  Assert.ThrowsExactlyAsync<UserNotFoundException>(async () =>
                {
                    await loginCommandHandler.Handle(command, CancellationToken.None);
                }
            );
        }

        [TestMethod]
        public async Task LoginCommandHandler_Should_Throw_InvalidCredentionalsException()
        {
            // ARRANGE(Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<Program>();
            IConfiguration configuration = configurationBuilder.Build();


            var context = new AppDbContext(options);
            UserRepository userRepository = new UserRepository(context);
            TokenService tokenService = new TokenService(configuration);

            RegisterCommandHandler registerCommandHandler = new RegisterCommandHandler(userRepository, passwordHasher, context);

            RegisterRequestDto registerRequestDto = new RegisterRequestDto("testuser", "Secret123!", "Test User");

            RegisterCommand redisterCommand = new RegisterCommand(registerRequestDto);

            await registerCommandHandler.Handle(redisterCommand, CancellationToken.None);


            LoginCommandHandler loginCommandHandler = new LoginCommandHandler(tokenService, userRepository, passwordHasher);

            LoginRequestDto loginRequestDto = new LoginRequestDto("testuser", "Secret124!");

            LoginCommand command = new LoginCommand(loginRequestDto);


            // ACT (Действие)

            // ASSERT (Проверка)
            await Assert.ThrowsExactlyAsync<InvalidCredentionalsException>(async () =>
            {
                await loginCommandHandler.Handle(command, CancellationToken.None);
            }
            );
        }

        [TestMethod]
        public async Task LoginCommandHandler_Should_Return_Token()
        {
            // ARRANGE(Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<Program>();
            IConfiguration configuration = configurationBuilder.Build();

            var context = new AppDbContext(options);
            UserRepository userRepository = new UserRepository(context);
            TokenService tokenService = new TokenService(configuration);
            RegisterCommandHandler registerCommandHandler = new RegisterCommandHandler(userRepository, passwordHasher, context);

            RegisterRequestDto registerRequestDto = new RegisterRequestDto("testuser", "Secret123!", "Test User");

            RegisterCommand redisterCommand = new RegisterCommand(registerRequestDto);

            await registerCommandHandler.Handle(redisterCommand, CancellationToken.None);

            LoginCommandHandler loginCommandHandler = new LoginCommandHandler(tokenService, userRepository, passwordHasher);

            LoginRequestDto loginRequestDto = new LoginRequestDto("testuser", "Secret123!");

            LoginCommand command = new LoginCommand(loginRequestDto);


            // ACT (Действие)

            var token = await loginCommandHandler.Handle(command, CancellationToken.None);

            // ASSERT (Проверка)
            Assert.AreNotEqual(String.Empty, token);

        }
    }
}
