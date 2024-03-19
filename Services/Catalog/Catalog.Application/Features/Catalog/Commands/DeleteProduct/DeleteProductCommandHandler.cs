using AutoMapper;
using Catalog.Application.Contracts.Persistence;
using Catalog.Application.Features.Catalog.Commands.CreateProduct;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Catalog.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, string>
    {

        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteProductCommandHandler> _logger;
        public DeleteProductCommandHandler(IProductRepository productRepository,IMapper mapper,ILogger<DeleteProductCommandHandler> logger)
        {
            _repository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var response = await _repository.DeleteAsync(request.Id);
            if (!response)
            {
                throw new Exception("Cannot delete new product");
               
            }
            return "Deleted";
        }
    }
}
