using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Tasks.Handlers;
using PersonalFinance.Infrastructure;
using PersonalFinance.Infrastructure.Repositories;
using PersonalFinanceApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class CurrentUserTests
    {
        [TestMethod]
        public void CurrentUser_Should_Return_The_Same_UserId()
        {
            // ARRANGE(Подготовка)
           
            var guid = Guid.NewGuid();

            DefaultHttpContext defaultHttpContext = new DefaultHttpContext();
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var claim = new Claim(ClaimTypes.NameIdentifier, guid.ToString());
            var claimsIdentity = new ClaimsIdentity(new Claim[] { claim });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            defaultHttpContext.User = claimsPrincipal;

            httpContextAccessor.HttpContext = defaultHttpContext;

            CurrentUser currentUser = new CurrentUser(httpContextAccessor);



            // ACT (Действие)

            var result = currentUser.UserId;

            // ASSERT (Проверка)
            Assert.AreEqual(guid, result);

        }

        [TestMethod]
        public void CurrentUser_Should_Throw_UnauthorizedAccessException()
        {
            // ARRANGE(Подготовка)

            var guid = Guid.NewGuid();

            DefaultHttpContext defaultHttpContext = new DefaultHttpContext();
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var claim = new Claim(ClaimTypes.Name, guid.ToString());
            var claimsIdentity = new ClaimsIdentity(new Claim[] { claim });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            defaultHttpContext.User = claimsPrincipal;

            httpContextAccessor.HttpContext = defaultHttpContext;

            CurrentUser currentUser = new CurrentUser(httpContextAccessor);



            // ACT (Действие)

            // ASSERT (Проверка)
            Assert.ThrowsExactly<UnauthorizedAccessException>(() => currentUser.UserId);

        }

        [TestMethod]
        public void CurrentUser_Should_Throw_InvalidOperationException()
        {
            // ARRANGE(Подготовка)


            DefaultHttpContext defaultHttpContext = new DefaultHttpContext();
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var claim = new Claim(ClaimTypes.NameIdentifier, "123");
            var claimsIdentity = new ClaimsIdentity(new Claim[] { claim });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            defaultHttpContext.User = claimsPrincipal;

            httpContextAccessor.HttpContext = defaultHttpContext;

            CurrentUser currentUser = new CurrentUser(httpContextAccessor);



            // ACT (Действие)


            // ASSERT (Проверка)
            Assert.ThrowsExactly<InvalidOperationException>(() => currentUser.UserId);

        }
    }
}
