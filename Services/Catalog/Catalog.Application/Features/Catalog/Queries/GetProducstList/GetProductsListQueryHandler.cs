using AutoMapper;
using Catalog.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Catalog.Queries.GetProducstList
{
    internal class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, List<ProductVM>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductsListQueryHandler> _logger;

        public GetProductsListQueryHandler(
        IProductRepository productRepository, IMapper mapper, ILogger<GetProductsListQueryHandler> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }
        public async Task<List<ProductVM>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var productLists = await _productRepository.GetAllAsync();
            return _mapper.Map<List<ProductVM>>(productLists);
        }
    }
  
}
