using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.UnitTests
{
 
    public class DeleteOrderHandlerTests
    {
        private readonly DeleteOrderCommandHandler _handler;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<DeleteOrderCommandHandler>> _loggerMock;
        public DeleteOrderHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<DeleteOrderCommandHandler>>();
            _handler = new DeleteOrderCommandHandler(_orderRepositoryMock.Object,_mapperMock.Object,_loggerMock.Object);
        }

        [Fact]
        public async Task DeleteOrderSuccess_WithOrderExist_AndReturnUnitValue()
        {
            // Arrange
            // tạo  1 record fake để test 
            var orderId = 1;
            var orderToDelete = new Order();
            orderToDelete.SetupForUnitTest(orderId);

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(orderToDelete);

            // Act
            var result = await _handler.Handle(new DeleteOrderCommand { Id = orderId }, CancellationToken.None);

         
            // Assert
            Assert.NotNull(result);
          
        }
        
        [Fact]
        public async Task DeleteOrderSuccess_WithOrderNotExist_AndReturnUnitValue()
        {

            var orderId = 1;
            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync((Order)null); // giả lập ko tìm thấy

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _handler.Handle(new DeleteOrderCommand { Id = orderId }, CancellationToken.None);
            });

        }
        
    }
}
