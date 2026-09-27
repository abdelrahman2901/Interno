using E_Commerce_Interno.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
namespace E_Commerce_Inern_Project.Core.Features.Products.Commands.CreateProductCommand
{
        public class ProductRequest : IRequest<Result<bool>>
    {
        public string? ProductName { get; set; }
         public   Guid? CategoryID { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public Guid SizeID { get; set; }
        [SwaggerSchema(Nullable = true)]
        public Guid? ColorID { get; set; }
        [SwaggerSchema(Nullable = true)]
        public string? ColorHexCode { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; }
        public IFormFile? ProductImage { get; set; }
    }
}