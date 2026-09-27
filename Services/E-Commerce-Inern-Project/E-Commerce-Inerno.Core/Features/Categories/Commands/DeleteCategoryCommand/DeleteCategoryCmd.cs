using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Categories.Commands.DeleteCategoryCommand
{
    public record DeleteCategoryCmd(Guid CatID) : IRequest<Result<bool>>;
}
