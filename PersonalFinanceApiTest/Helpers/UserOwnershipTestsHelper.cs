using Microsoft.AspNetCore.Mvc.Testing;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace PersonalFinanceApiTest.Helpers
{
    public class UserOwnershipTestsHelper
    {
        public async Task<HttpClient> CreateAuthenticatedClientAsync(CustomWebApplicationFactory factory, string login)
        {
            var client = factory.CreateClient();

            var responseUser = await client.PostAsJsonAsync(
             "/api/auth/register",
               new RegisterRequestDto(login, "Secret123!", login)
           );

            Assert.AreEqual(HttpStatusCode.Created, responseUser.StatusCode);

            var responseLoginClient = await client.PostAsJsonAsync(
              "api/auth/login",
                new LoginRequestDto(login, "Secret123!")
            );

            var tokenClient = await responseLoginClient.Content.ReadAsStringAsync();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenClient);

            return client;
        }

        public async Task<AccountItemDto> CreateAccountAsync(HttpClient clientA, Currency currency)
        {
            var responseCreateAccount = await clientA.PostAsJsonAsync(
              "api/accounts",
                new CreateAccountDto("accountA", currency.Id)
            );

            Assert.AreEqual(HttpStatusCode.Created, responseCreateAccount.StatusCode);

            var createdAccount = await responseCreateAccount.Content.ReadFromJsonAsync<AccountItemDto>();

            Assert.IsNotNull(createdAccount);

            return createdAccount;
        }

    }
}
