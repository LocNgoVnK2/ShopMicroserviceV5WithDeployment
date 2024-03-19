using AutoMapper;
using Catalog.Application.Contracts.Persistence;
using Catalog.Application.Features.Catalog.Queries.GetProducstList;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Catalog.Queries.GetProductById
{
    internal class GetProductsListByIdQueryHandler : IRequestHandler<GetProductsListByIdQuery, ProductVMGetFromById>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductsListByIdQueryHandler> _logger;

        public GetProductsListByIdQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductsListByIdQueryHandler> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }
        public async Task<ProductVMGetFromById> Handle(GetProductsListByIdQuery request, CancellationToken cancellationToken)
        {
            var productLists = await _productRepository.GetByIdAsync(request.Id);
            return _mapper.Map<ProductVMGetFromById>(productLists);
        }
    }
}
