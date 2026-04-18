using E_Commerce_Inern_Project.Core.Features.Category.Commands.CreateCategoryCommad;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.CategoryValidation
{
    public class CategoryRequestValidation : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidation()
        {
            RuleFor(p => p.CategoryName).NotEmpty().WithMessage("CategoryName is required");
            //RuleFor(p => p.CategoryImage).NotEmpty().WithMessage("Category Image Is Required");
            
        }
    }
}
