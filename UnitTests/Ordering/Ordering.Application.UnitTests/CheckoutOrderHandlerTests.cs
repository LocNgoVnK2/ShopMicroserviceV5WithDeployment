using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.UnitTests
{
    public class CheckoutOrderHandlerTests
    {
        private readonly CheckoutOrderCommandHandler _handler;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ILogger<CheckoutOrderCommandHandler>> _loggerMock;

        public CheckoutOrderHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _emailServiceMock = new Mock<IEmailService>();
            _loggerMock = new Mock<ILogger<CheckoutOrderCommandHandler>>();

            _handler = new CheckoutOrderCommandHandler(
                _orderRepositoryMock.Object,
                _mapperMock.Object,
                _emailServiceMock.Object,
                _loggerMock.Object
                ); ;
        }

        [Fact]
        public async void CheckoutOrderSuccess_ShouldNotHaveException_AndReturnNewOrderId()
        {
            var newOrder = new Order();
            newOrder.SetupForUnitTest(1);

            _orderRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Order>())).ReturnsAsync(newOrder);

            var newOrderId = await _handler.Handle(It.IsAny<CheckoutOrderCommand>(), It.IsAny<CancellationToken>());
         
            Assert.Equal(1, newOrderId);
        }
    }
}