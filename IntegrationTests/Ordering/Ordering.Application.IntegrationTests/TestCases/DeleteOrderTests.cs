using AutoFixture;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.IntegrationTests.TestCases
{
    [Collection(ApplicationTestFixture.ApplicationTestFixtureCollection)]
    public class DeleteOrderTests
    {
        private readonly Fixture _fakeData;
        private readonly IMediator _mediator;
        private readonly OrderContext _orderContext;
        private readonly ApplicationTestFixture _fixture;
        public DeleteOrderTests(ApplicationTestFixture fixture)
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
        public async Task TestDeleteOrder()
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
            //Create check outcommand 
            var result = await _mediator.Send(checkoutOrderCommand);
            var order = _orderContext.Orders.OrderByDescending(x => x.LastModifiedDate).FirstOrDefault(x => x.UserName == "john_doe");
            Assert.NotNull(order);
            Assert.Equal(order.Id, result);

            //delete it
            var deleteOrderCommand = new DeleteOrderCommand() { 
                Id = order.Id
            };
            var resultDelete = await _mediator.Send(deleteOrderCommand);
            Assert.IsType<Unit>(resultDelete);


            // check it exist
            var getOrderList = new GetOrdersListQuery(checkoutOrderCommand.UserName, "", new Guid());
            var resultFromGet = await _mediator.Send(getOrderList);
            Assert.Empty(resultFromGet);
        }
    }
}
