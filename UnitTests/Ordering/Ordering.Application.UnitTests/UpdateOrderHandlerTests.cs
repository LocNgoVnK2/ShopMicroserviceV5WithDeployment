using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.UnitTests
{

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

            var orderId = 1;
            var order = new Order();
            order.SetupForUnitTest(orderId);

            _repository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);

            var result = await _handler.Handle(new UpdateOrderCommand { Id = orderId }, CancellationToken.None);

         
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
}
