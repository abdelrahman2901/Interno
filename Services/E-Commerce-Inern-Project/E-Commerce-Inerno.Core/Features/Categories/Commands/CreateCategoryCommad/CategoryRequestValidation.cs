using FluentValidation;

namespace E_Commerce_Inerno.Core.Features.Categories.Commands.CreateCategoryCommad
{
    public class CategoryRequestValidation : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidation()
        {
            RuleFor(p => p.CategoryName).NotEmpty().WithMessage("CategoryName is required");
        }
    }
}
