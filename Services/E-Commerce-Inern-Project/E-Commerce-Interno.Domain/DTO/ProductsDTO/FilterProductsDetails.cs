namespace E_Commerce_Interno.Domain.DTO.ProductsDTO
{
    public class FilterProductsDetails
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public decimal? Price { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Sort { get; set; }
    }
}
