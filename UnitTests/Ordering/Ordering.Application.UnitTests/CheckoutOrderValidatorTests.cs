using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using System;
using Xunit.Sdk;

namespace Ordering.Application.UnitTests
{
    public class CheckoutOrderValidatorTests
    {
        private readonly CheckoutOrderCommandValidator _validator;

        public CheckoutOrderValidatorTests()
        {
            _validator = new CheckoutOrderCommandValidator();
        }

        [Fact]
        public void It_ShouldShowMessage_If_UsernameEmpty()
        {
            var command = new CheckoutOrderCommand();

           var result =  _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Equal("{UserName} is required.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void It_ShouldShowMessage_If_UsernameOver50Characters()
        {
            var command = new CheckoutOrderCommand();
            command.UserName = RandomString(60);

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Equal("{UserName} must not exceed 50 characters.", result.Errors[0].ErrorMessage);
        }

        private string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
