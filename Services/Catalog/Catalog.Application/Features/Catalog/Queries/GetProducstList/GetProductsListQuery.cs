using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Catalog.Queries.GetProducstList
{
    public class GetProductsListQuery : IRequest<List<ProductVM>>
    {
    
    }
}
