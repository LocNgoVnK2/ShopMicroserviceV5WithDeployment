using AutoMapper;
using Catalog.Application.Features.Catalog.Commands.CreateProduct;
using Catalog.Application.Features.Catalog.Queries.GetProducstList;
using Catalog.Application.Features.Catalog.Queries.GetProductById;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductVM>().ReverseMap();
            CreateMap<Product, ProductVMGetFromById>().ReverseMap();
            CreateMap<Product, CreateProductCommand>().ReverseMap();
        } 
    }
}
