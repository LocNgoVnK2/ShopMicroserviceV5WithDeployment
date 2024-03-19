using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Catalog.Application.Contracts.Persistence;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Catalog.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, string>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommandHandler> _logger;
        public CreateProductCommandHandler(IProductRepository repository, IMapper mapper, ILogger<CreateProductCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Product>(request);
            var viewProduct = await _repository.AddAsync(productEntity);
            if (viewProduct == null)
            {
                throw new Exception("Cannot create new product");
            }
            _logger.LogInformation($"Order {viewProduct.Id} is successfully created.");
            return viewProduct.Id;
        }

      
    }
}
