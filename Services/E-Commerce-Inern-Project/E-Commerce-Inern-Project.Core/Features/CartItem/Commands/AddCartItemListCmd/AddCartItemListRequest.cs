using E_Commerce_Inern_Project.Core.Common;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.AddCartItemListCmd
{
    public class AddCartItemListRequest : IRequest<Result<bool>>
    {
        [JsonIgnore]
        [SwaggerSchema(Nullable = true)]
        public Guid UserID { get; set; }
        public List<Guid> ProductIDs { get; }
    }
}
