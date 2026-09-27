using E_Commerce_Interno.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce_Inerno.Core.Features.Products.Commands.UpdateProductCommand
{
    //public record ProductUpdateRequest(Guid ProductID, Guid CategoryID, string ProductName);
    public class ProductUpdateRequest : IRequest<Result<bool>>
    {
        public Guid? ProductID { get; set; }
        public string? ProductName { get; set; }
        public Guid? CategoryID { get; set; }
        public decimal Price { get; set; }
        public Guid SizeID { get; set; }
        [SwaggerSchema(Nullable = true)]
        public Guid? ColorID { get; set; }
        [SwaggerSchema(Nullable = true)]
        public string? ColorHexCode { get; set; }
        public decimal? SalePrice { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; }
        public IFormFile? ProductImage { get; set; }
    }
}