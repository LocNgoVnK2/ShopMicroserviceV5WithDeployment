using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Catalog.Queries.GetProductById
{
    public class GetProductsListByIdQuery : IRequest<ProductVMGetFromById>
    {
        public string Id { get; set; }
        public GetProductsListByIdQuery(string id)
        {
          Id = id;
        }
    }
}
