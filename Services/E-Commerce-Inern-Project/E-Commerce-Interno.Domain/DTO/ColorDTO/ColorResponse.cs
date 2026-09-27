using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce_Interno.Domain.DTO.ColorDTO
{
    public class ColorResponse
    {
        public Guid ColorID { get; set; }
        [SwaggerSchema(Nullable =true)]
        public string? ColorName { get; set; }
        [SwaggerSchema(Nullable =true)]
        public string? ColorHexCode { get; set; }
    }
}
