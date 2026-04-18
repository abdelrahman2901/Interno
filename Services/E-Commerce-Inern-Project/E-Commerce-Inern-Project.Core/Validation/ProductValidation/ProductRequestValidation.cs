using E_Commerce_Inern_Project.Core.Features.Product.Commands.CreateProductCommand;
using FluentValidation;
namespace E_Commerce_Inern_Project.Core.Validation.ProductValidation
{
    public class ProductRequestValidation :AbstractValidator<ProductRequest>
    {
      public ProductRequestValidation() {
            RuleFor(p=>p.ProductName).NotEmpty().WithMessage("Product Name Cant Be Empty");
            RuleFor(p=>p.CategoryID).NotEmpty().WithMessage("CategoryID Cant Be Empty");
            RuleFor(p=>p.Price).NotEmpty().WithMessage("Price Cant Be Empty");
            RuleFor(p=>p.Price).GreaterThan(0).WithMessage("Price Must Be Greater Than 0");
            RuleFor(p=>p.ProductImage).NotEmpty().WithMessage("Product Image Cant Be Empty");
            RuleFor(p=>p.SizeID).NotEmpty().WithMessage("Size is Required");
            RuleFor(p=>p.ColorID).NotEmpty().WithMessage("Color is Required");
        }
    }
}
