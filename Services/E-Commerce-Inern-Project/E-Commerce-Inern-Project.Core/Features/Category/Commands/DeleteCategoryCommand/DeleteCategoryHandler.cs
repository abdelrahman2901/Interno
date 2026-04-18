using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICategoryServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Category.Commands.DeleteCategoryCommand
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCmd, Result<bool>>
    {
        private readonly ICategoryService _catService;
        public DeleteCategoryHandler(ICategoryService CatService)
        {
            _catService = CatService;
        }
        public async Task<Result<bool>> Handle(DeleteCategoryCmd request, CancellationToken cancellationToken)
        {
            return await _catService.DeleteCategory(request.CatID);
        }
    }
}
