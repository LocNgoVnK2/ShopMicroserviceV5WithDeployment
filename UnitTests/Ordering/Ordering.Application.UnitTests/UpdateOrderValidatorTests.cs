using FluentValidation;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.UnitTests
{
    public class UpdateOrderValidatorTests
    {
        private readonly UpdateOrderCommandValidator _validator;

        public UpdateOrderValidatorTests()
        {
            _validator = new UpdateOrderCommandValidator();
        }

        [Fact]
        public void It_ShouldShowMessage_If_UsernameEmpty()
        {
            var command = new UpdateOrderCommand();

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Equal("{UserName} is required.", result.Errors[0].ErrorMessage);
        }
        [Fact]
        public void It_ShouldShowMessage_If_UsernameOver50Characters()
        {
            var command = new UpdateOrderCommand();
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
