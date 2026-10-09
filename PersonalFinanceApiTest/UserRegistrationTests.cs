using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Tasks.Handlers;
using PersonalFinance.Domain;
using PersonalFinance.Infrastructure;
using PersonalFinance.Infrastructure.Concurency;
using PersonalFinance.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class UserRegistrationTests
    {
        [TestMethod]
        public async Task RegisterCommandHandler_Should_CreateUser()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();


            var context = new AppDbContext(options);
            UserRepository userRepository = new UserRepository(context);
            RegisterCommandHandler registerCommandHandler = new RegisterCommandHandler(userRepository, passwordHasher, context);

            RegisterRequestDto registerRequestDto = new RegisterRequestDto("testuser5", "Secret123!", "Test User");

            RegisterCommand command = new RegisterCommand(registerRequestDto);


            // ACT (Действие)
            var guid = await registerCommandHandler.Handle(command, CancellationToken.None);


            // ASSERT (Проверка)
            Assert.AreNotEqual(Guid.Empty, guid);

            var addedUser = await userRepository.GetUserAsync("testuser5");

            Assert.IsNotNull(addedUser);
            Assert.AreEqual(guid, addedUser.Id);
            Assert.AreEqual("testuser5", addedUser.Login);
        }

        [TestMethod]
        public async Task RegisterCommandHandler_Should_Throw_Exception_When_Login_duplicate()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();


            var context = new AppDbContext(options);
            UserRepository userRepository = new UserRepository(context);
            RegisterCommandHandler registerCommandHandler = new RegisterCommandHandler(userRepository, passwordHasher, context);

            RegisterRequestDto registerRequestDto = new RegisterRequestDto("testuser7", "Secret123!", "Test User");

            RegisterCommand command = new RegisterCommand(registerRequestDto);

            RegisterCommand command2 = new RegisterCommand(registerRequestDto);


            // ACT (Действие)
            var guid = await registerCommandHandler.Handle(command, CancellationToken.None);



            // ASSERT (Проверка)
            await Assert.ThrowsExactlyAsync<LoginIsNotUniqueException>(async () =>
                 {
                     await registerCommandHandler.Handle(command2, CancellationToken.None);
                 }
                 );
        }

        [TestMethod]
        public async Task RegisterCommandHandler_Password_Is_Really_Hash()
        {
            // ARRANGE(Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();


            var context = new AppDbContext(options);
            UserRepository userRepository = new UserRepository(context);
            RegisterCommandHandler registerCommandHandler = new RegisterCommandHandler(userRepository, passwordHasher, context);

            RegisterRequestDto registerRequestDto = new RegisterRequestDto("testuser8", "Secret123!", "Test User");

            RegisterCommand command = new RegisterCommand(registerRequestDto);


            // ACT (Действие)
            await registerCommandHandler.Handle(command, CancellationToken.None);

            var addedUser = await userRepository.GetUserAsync("testuser8");

            // ASSERT (Проверка)
            Assert.AreNotEqual("Secret123!", addedUser.PasswordHash);
            var result = passwordHasher.VerifyHashedPassword(addedUser, addedUser.PasswordHash, "Secret123!");
            Assert.AreEqual(PasswordVerificationResult.Success, result);
        }

        [TestMethod]
        public async Task RegisterEndpoint_Should_Return_201()
        {
            // ARRANGE(Подготовка)
            WebApplicationFactory<Program> webApplicationFactory = new WebApplicationFactory<Program>();
            
           

            var client = webApplicationFactory.CreateClient();


            // ACT (Действие)
            var response = await client.PostAsJsonAsync(
              "/api/auth/register",
                new RegisterRequestDto("testuser19", "Secret123!", "Test User")
            );

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var guid = await response.Content.ReadFromJsonAsync<Guid>();

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var user = await db.Users.FirstOrDefaultAsync(u => u.Login == "testuser19");


            // ASSERT (Проверка)
           

            Assert.AreEqual(guid, user.Id);

        }
    }
}

