using App.Application.Contracts.Persistance;
using App.Application.Features.Categories.Update;
using FluentValidation;

namespace App.Services.Categories.Update
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategoryRequestValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x)
                .MustAsync(MustUniqueCategoryNameAsync).WithMessage("Kategori ismi veritabanında bulunmaktadır.");

            RuleFor(x => x.Name)
                //.NotNull().WithMessage("Kategori ismi gereklidir.")
                .NotEmpty().WithMessage("Kategori ismi gereklidir.");
        }

        private async Task<bool> MustUniqueCategoryNameAsync(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var anyCategory = await _categoryRepository.AnyAsync(x => x.Name == request.Name);

            return !anyCategory;
        }
    }
}
