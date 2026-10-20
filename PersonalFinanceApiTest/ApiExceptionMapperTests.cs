using Microsoft.AspNetCore.Http;
using PersonalFinance.Domain.Exceptions;
using PersonalFinanceApi.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class ApiExceptionMapperTests
    {
        private readonly ApiExceptionMapper _mapper = new();
        [TestMethod]
        public void Map_InsufficientFundsException_ReturnsBadRequest()
        {
            // Arrange
            var exception = new InsufficientFundsException("Not enough funds");

            // Act
            var result = _mapper.Map(exception);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.Status);
            Assert.AreEqual("Insufficient funds", result.Title);
            Assert.AreEqual("Not enough funds", result.Detail);
        }

        [TestMethod]
        public void Map_AccountAlreadyClosedException_ReturnsConflict()
        {
            string name = "123";
            // Arrange
            var exception = new AccountAlreadyClosedException(name);

            // Act
            var result = _mapper.Map(exception);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status409Conflict, result.Status);
            Assert.AreEqual("Account already closed", result.Title);
            Assert.AreEqual("Account with name '123' already closed.", result.Detail);
        }

        [TestMethod]
        public void Map_InvalidDepositAmountException_ReturnsBadRequest()
        {
            // Arrange
            var exception = new InvalidDepositAmountException(100);

            // Act
            var result = _mapper.Map(exception);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.Status);
            Assert.AreEqual("Invalid deposit amount", result.Title);
            Assert.AreEqual("Invalid deposit amount: 100", result.Detail);
        }

        [TestMethod]
        public void Map_InvalidWithdrawAmountException_ReturnsBadRequest()
        {
            // Arrange
            var exception = new InvalidWithdrawAmountException(100);

            // Act
            var result = _mapper.Map(exception);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.Status);
            Assert.AreEqual("Invalid withdraw amount", result.Title);
            Assert.AreEqual("Invalid withdraw amount: 100", result.Detail);
        }

        [TestMethod]
        public void Map_UnknownException_ReturnsNull()
        {
            // Arrange
            var exception = new InvalidOperationException("Unexpected error");

            // Act
            var result = _mapper.Map(exception);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Constructor_WithDetail_SetsMessage()
        {
            // Arrange
            const string expectedMessage = "Not enough funds";

            // Act
            var exception = new InsufficientFundsException(expectedMessage);

            // Assert
            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}
