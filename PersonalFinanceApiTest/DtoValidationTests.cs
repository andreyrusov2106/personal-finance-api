using PersonalFinance.Application.DTOs;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class DtoValidationTests
    {
        [TestMethod]
        public void DepositAccountDto_Should_RejectAmountBelowMinimum()
        {
            // ARRANGE (Подготовка)

            var depositAccountDto = new DepositAccountDto(0m);

            var context = new ValidationContext(depositAccountDto);
            var results = new List<ValidationResult>();
            // ACT (Действие)
            var amountParameter = typeof(DepositAccountDto)
            .GetConstructors()
            .Single()
            .GetParameters()
            .Single(p => p.Name == nameof(DepositAccountDto.Amount));

            var rangeAttribute = (RangeAttribute)Attribute.GetCustomAttribute(
                amountParameter,
                typeof(RangeAttribute))!;

            var isValid = Validator.TryValidateValue(
                depositAccountDto.Amount,
                context,
                results,
                new[] { rangeAttribute });



            // ASSERT (Проверка)

            Assert.IsFalse(isValid);

        }

        [TestMethod]
        public void DepositAccountDto_Is_Valid_With_Amount_99_999_999()
        {
            // ARRANGE (Подготовка)

            var depositAccountDto = new DepositAccountDto(99_999_999.99m);

            var context = new ValidationContext(depositAccountDto);
            var results = new List<ValidationResult>();
            // ACT (Действие)
            var amountParameter = typeof(DepositAccountDto)
            .GetConstructors()
            .Single()
            .GetParameters()
            .Single(p => p.Name == nameof(DepositAccountDto.Amount));

                var rangeAttribute = (RangeAttribute)Attribute.GetCustomAttribute(
                    amountParameter,
                    typeof(RangeAttribute))!;

            var isValid = Validator.TryValidateValue(
                depositAccountDto.Amount,
                context,
                results,
                new[] { rangeAttribute });



            // ASSERT (Проверка)

            Assert.IsTrue(isValid);

        }


        [TestMethod]
        public void DepositAccountDto_Is_Not_Valid_With_Amount_100_000_00()
        {
            // ARRANGE (Подготовка)

            var depositAccountDto = new DepositAccountDto(100_000_000m);

            var context = new ValidationContext(depositAccountDto);
            var results = new List<ValidationResult>();
            // ACT (Действие)
            var amountParameter = typeof(DepositAccountDto)
            .GetConstructors()
            .Single()
            .GetParameters()
            .Single(p => p.Name == nameof(DepositAccountDto.Amount));

            var rangeAttribute = (RangeAttribute)Attribute.GetCustomAttribute(
                amountParameter,
                typeof(RangeAttribute))!;

            var isValid = Validator.TryValidateValue(
                depositAccountDto.Amount,
                context,
                results,
                new[] { rangeAttribute });



            // ASSERT (Проверка)

            Assert.IsFalse(isValid);

        }
    }
}
