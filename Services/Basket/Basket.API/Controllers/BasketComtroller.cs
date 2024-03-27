using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        
        private readonly DiscountGrpcService _discountGrpcService;
        
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketController> _logger;
        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IPublishEndpoint publishEndpoint, IMapper mapper, ILogger<BasketController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            Guid eventId = Guid.NewGuid();
            _logger.LogInformation("Start Controller {controller} Action {action}  with {eventId}, username={username}"
                , nameof(BasketController), nameof(GetBasket), eventId, userName);
            var basket = await _repository.GetBasket(userName);
            if (basket == null)
            {
                _logger.LogInformation("Controller {controller} Action {action}  with {eventId}, username={username}. Not found",
                    nameof(BasketController), nameof(GetBasket), eventId, userName);
            }
            else
            {
                _logger.LogInformation("Controller {controller} Action {action}  with {eventId}, username={username}. Found",
                    nameof(BasketController), nameof(GetBasket), eventId, userName);
            }

            _logger.LogInformation("End Controller {controller} Action {action}  with {eventId}, username={username}",
                nameof(BasketController), nameof(GetBasket), eventId, userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }
        //way 1
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // Communicate with Discount.Grpc and calculate lastest prices of products into sc
            Guid eventId = Guid.NewGuid();
            _logger.LogInformation("Start Controller {controller} Action {action} with {eventId}",
                nameof(BasketController), nameof(UpdateBasket), eventId);

            try
            {
                // Communicate with Discount.Grpc and calculate lastest prices of products into sc
                foreach (var item in basket.Items)
                {
                    _logger.LogInformation("Foreach in Controller {controller} Action {action} with {eventId} , Item :{item}",
                   nameof(BasketController), nameof(UpdateBasket), eventId,item);
                    var coupon = await _discountGrpcService.GetDiscount(item.ProductName, eventId);
                    _logger.LogInformation("calculating in Controller {controller} Action {action} with {eventId} , Item :{item} , BasePrice: {basePrice} , Coupon amount :{couponAmount}",
               nameof(BasketController), nameof(UpdateBasket), eventId, item);
                    item.Price -= coupon.Amount;
                }
                // end communicate

                var updatedBasket = await _repository.UpdateBasket(basket);
                _logger.LogInformation("Controller {controller} Action {action} with {eventId}. Basket updated successfully",
                    nameof(BasketController), nameof(UpdateBasket), eventId);
                return Ok(updatedBasket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller {controller} Action {action} with {eventId}. Error updating basket: {errorMessage}",
                    nameof(BasketController), nameof(UpdateBasket), eventId, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating basket");
            }
            finally
            {
                _logger.LogInformation("End Controller {controller} Action {action} with {eventId}",
                    nameof(BasketController), nameof(UpdateBasket), eventId);
            }

    
        }
 
            [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
        
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price            
            // Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket

            // get existing basket with total price
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }
            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice; // type of basket is shopping cart

            await _publishEndpoint.Publish<BasketCheckoutEvent>(eventMessage);
            // remove the basket
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
        
    }
}
