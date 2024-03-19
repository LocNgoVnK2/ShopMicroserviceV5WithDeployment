using Catalog.Application.Features.Catalog.Commands.CreateProduct;
using Catalog.Application.Features.Catalog.Commands.DeleteProduct;
using Catalog.Application.Features.Catalog.Queries.GetProducstList;
using Catalog.Application.Features.Catalog.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catalog.APIcqrs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(
            IMediator mediator,
            ILogger<CatalogController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }
        [HttpGet(Name = "GetAllProducts")]
        [ProducesResponseType(typeof(IEnumerable<ProductVM>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductVM>>> GetAllProducts()
        {
            var query = new GetProductsListQuery();
            var products = await _mediator.Send(query);

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductVMGetFromById), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVMGetFromById>> GetProductById(string id)
        {
            var query = new GetProductsListByIdQuery(id);
            var product = await _mediator.Send(query);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> CreateProduct([FromBody] CreateProductCommand productQuery)
        {
            var query = productQuery;
            var newId = await _mediator.Send(query);
            return Ok(new{ id = newId });

        }
        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            DeleteProductCommand command = new DeleteProductCommand() { Id = id};
            var inforResponse = await _mediator.Send(command);
            return Ok(inforResponse);
        }
    }
}
