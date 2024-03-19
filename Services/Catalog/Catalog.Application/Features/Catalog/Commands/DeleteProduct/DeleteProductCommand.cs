using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Catalog.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
}
