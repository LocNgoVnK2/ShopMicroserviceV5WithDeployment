using AutoFixture;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.IntegrationTests.TestCases
{
    [Collection(ApplicationTestFixture.ApplicationTestFixtureCollection)]
    public class UpdateOrderTests
    {
        private readonly Fixture _fakeData;
        private readonly IMediator _mediator;
        private readonly OrderContext _orderContext;
        private readonly ApplicationTestFixture _fixture;
        public UpdateOrderTests(ApplicationTestFixture fixture)
        {
            _mediator = fixture.Services.GetRequiredService<IMediator>();
            _orderContext = fixture.Services.GetRequiredService<OrderContext>();

            _fakeData = new Fixture();
            _fakeData.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fakeData.Behaviors.Remove(b));
            _fakeData.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture = fixture;
        }

        [Fact]
        public async Task TestUpdateOrder()
        {
            var checkoutOrderCommand = new CheckoutOrderCommand
            {
                UserName = "john_doe",
                TotalPrice = 100000,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                AddressLine = "123 Main St",
                Country = "USA",
                State = "CA",
                ZipCode = "90001",
                CardName = "John Doe",
                CardNumber = "1234567890123456",
                Expiration = "12/25",
                CVV = "123",
                PaymentMethod = 1 // Assume 1 represents Credit Card payment method
            };

            var result = await _mediator.Send(checkoutOrderCommand);
           

            var orderAfterUpdate = _orderContext.Orders.OrderByDescending(x => x.LastModifiedDate).FirstOrDefault(x => x.UserName == "john_doe");
            Assert.NotNull(orderAfterUpdate);
            Assert.Equal(orderAfterUpdate.Id, result);

            UpdateOrderCommand updateOrderCommand = new UpdateOrderCommand()
            {
                Id = orderAfterUpdate.Id,
                UserName = "john_doe",
                TotalPrice = 100000,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                AddressLine = "123 Main St",
                Country = "a",
                State = "a",
                ZipCode = "a",
                CardName = "a",
                CardNumber = "1234567890123456",
                Expiration = "12/25",
                CVV = "123",
                PaymentMethod = 1 // Assume 1 represents Credit Card payment method
            };
            var resultFromUpdate = await _mediator.Send(updateOrderCommand);

            Assert.IsType<Unit>(resultFromUpdate);
            var orderBeforeupdate = _orderContext.Orders.OrderByDescending(x => x.LastModifiedDate).FirstOrDefault(x => x.UserName == "john_doe");

            Assert.NotEqual(checkoutOrderCommand.Country, orderBeforeupdate.Country);
        }
    }
}
