using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Enums;
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

        public async Task<AccountItemDto> CreateAccountAsync(HttpClient clientA, Currency currency, string name)
        {
            var responseCreateAccount = await clientA.PostAsJsonAsync(
              "api/accounts",
                new CreateAccountDto(name, currency.Id)
            );

            Assert.AreEqual(HttpStatusCode.Created, responseCreateAccount.StatusCode);

            var createdAccount = await responseCreateAccount.Content.ReadFromJsonAsync<AccountItemDto>();

            Assert.IsNotNull(createdAccount);

            return createdAccount;
        }

        public async Task<AccountItemDto> DepositAccountAsync(HttpClient clientA, Guid accountId, decimal ammount)
        {
            var responseDepositAccount = await clientA.PostAsJsonAsync(
              $"api/accounts/{accountId}/deposit",
                new DepositAccountDto(ammount)
            );

            Assert.AreEqual(HttpStatusCode.OK, responseDepositAccount.StatusCode);

            var depositedAccount = await responseDepositAccount.Content.ReadFromJsonAsync<AccountItemDto>();

            Assert.IsNotNull(depositedAccount);

            return depositedAccount;
        }

        public async Task<AccountItemDto> WithdrawAccountAsync(HttpClient clientA, Guid accountId, decimal ammount)
        {
            var responseDepositAccount = await clientA.PostAsJsonAsync(
              $"api/accounts/{accountId}/withdraw",
                new DepositAccountDto(ammount)
            );

            Assert.AreEqual(HttpStatusCode.OK, responseDepositAccount.StatusCode);

            var withdrawalAccount = await responseDepositAccount.Content.ReadFromJsonAsync<AccountItemDto>();

            Assert.IsNotNull(withdrawalAccount);

            return withdrawalAccount;
        }

        public async Task TransferAsync(HttpClient clientA, Guid accountIdFrom, Guid accountIdTo, decimal ammount)
        {
            var responseTransferAccount = await clientA.PostAsJsonAsync(
              $"api/accounts/{accountIdFrom}/transfer",
                new TransferAccountDto(accountIdTo, ammount)
            );

            Assert.AreEqual(HttpStatusCode.NoContent, responseTransferAccount.StatusCode);
        }


        public async Task<PaginationHistoryDto> GetHistoryAsync(HttpClient clientA, 
            Guid accountId, 
            int page, 
            int pageSize,
            OperationType? type = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            var responseGetHistory = await clientA.GetAsync(
              $"api/accounts/{accountId}/history?page={page}" +
              $"&pageSize={pageSize}" +
              $"&type={type}&from={from}&to={to}"
            );

            Assert.AreEqual(HttpStatusCode.OK, responseGetHistory.StatusCode);

            var history = await responseGetHistory.Content.ReadFromJsonAsync<PaginationHistoryDto>();

            Assert.IsNotNull(history);

            return history;
        }

    }
}
