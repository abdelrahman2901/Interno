using E_Commerce_Inern_Project.Core.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace E_Commerce_Inern_Project.Core.Features.Product.Commands.CreateProductCommand
{
        public class ProductRequest : IRequest<Result<bool>>
    {
        public string? ProductName { get; set; }
         public   Guid? CategoryID { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public Guid SizeID { get; set; }
        public Guid ColorID { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; }
        public IFormFile? ProductImage { get; set; }
    }
            
}