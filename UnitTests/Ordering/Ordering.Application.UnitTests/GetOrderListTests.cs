using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.UnitTests
{
    /*
      public class UpdateOrderHandlerTests
    {
        private readonly UpdateOrderCommandHandler _handler;
        private readonly Mock<IOrderRepository> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<UpdateOrderCommandHandler>> _logger;
        public UpdateOrderHandlerTests()
        {
            _repository = new Mock<IOrderRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<UpdateOrderCommandHandler>>();
            _handler = new UpdateOrderCommandHandler(_repository.Object,_mapper.Object,_logger.Object);
        }
        [Fact]
        public async Task UpdateOrderSuccess_IfOrderExist_AndReturnUnitValue()
        {
            // set up ban đầu
            var orderId = 1;
            var order = new Order();
            order.SetupForUnitTest(orderId);

            _repository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);

            // thực hiện test
            var result = await _handler.Handle(new UpdateOrderCommand { Id = orderId }, CancellationToken.None);

            // so sánh kết quả
            Assert.NotNull(result);
            _repository.Verify(x => x.UpdateAsync(order), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderFail_IfOrderNotExist()
        {
            var orderId = 1;
            _repository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync((Order)null);
    
            var result = await _handler.Handle(new UpdateOrderCommand { Id = orderId }, CancellationToken.None);
            Assert.NotNull(result);
        }
    }
     */
    public class GetOrderListTests
    {
        private readonly Mock<IOrderRepository> _orderRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<GetOrdersListQueryHandler>> _logger;
        private readonly GetOrdersListQueryHandler _handler;
        public GetOrderListTests()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _mapper = new Mock<IMapper>();  
            _logger = new Mock<ILogger<GetOrdersListQueryHandler>>();
            _handler = new GetOrdersListQueryHandler(_orderRepository.Object,_mapper.Object,_logger.Object);
        }
        [Fact]
        public async Task GetOrderSuccess_AndReturnOrderObject()
        {
            // Arrange
            var userName = "user1";
            var orders = new List<Order>
            {
                new Order { UserName = userName}
            };
            _orderRepository.Setup(x=>x.GetOrdersByUserName(userName)).ReturnsAsync(orders);
            // _orderRepository.Setup(x => x.GetOrdersByUserName(userName)).ReturnsAsync(orders);

            // Act
            var result = await _handler.Handle(new GetOrdersListQuery(userName, It.IsAny<string>(), It.IsAny<Guid>()), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orders.Count, result.Count);
            _orderRepository.Verify(x => x.GetOrdersByUserName(userName), Times.Once);
        }

    }
}
