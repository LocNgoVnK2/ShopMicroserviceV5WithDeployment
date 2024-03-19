using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Catalog.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            /*
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(5).WithMessage("{UserName} must not exceed 50 characters.");
            */
            RuleFor(p => p.Description)
               .NotEmpty().WithMessage("{Description} is required.");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
    }
}
