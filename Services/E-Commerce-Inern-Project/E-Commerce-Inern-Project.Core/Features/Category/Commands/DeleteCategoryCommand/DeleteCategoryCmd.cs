using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Category.Commands.DeleteCategoryCommand
{
    public record DeleteCategoryCmd(Guid CatID) : IRequest<Result<bool>>;
}
