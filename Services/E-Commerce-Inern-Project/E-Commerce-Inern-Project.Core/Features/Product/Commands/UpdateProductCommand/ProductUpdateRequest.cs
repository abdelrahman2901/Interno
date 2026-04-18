using E_Commerce_Inern_Project.Core.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductCommand
{
    //public record ProductUpdateRequest(Guid ProductID, Guid CategoryID, string ProductName);
    public class ProductUpdateRequest : IRequest<Result<bool>>
    {
        public Guid? ProductID { get; set; }
        public string? ProductName { get; set; }
        public Guid? CategoryID { get; set; }
        public decimal Price { get; set; }
        public Guid SizeID { get; set; }
        public Guid? ColorID { get; set; }
        public decimal? SalePrice { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; }
        public IFormFile? ProductImage { get; set; }
    }
}