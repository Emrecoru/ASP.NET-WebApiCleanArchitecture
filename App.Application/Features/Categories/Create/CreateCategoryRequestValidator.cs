using App.Application.Contracts.Persistance;
using App.Application.Features.Categories.Create;
using FluentValidation;

namespace App.Services.Categories.Create
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        private readonly ICategoryRepository _categoryRepository;
        public CreateCategoryRequestValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Name)
                //.NotNull().WithMessage("Kategori ismi gereklidir.")
                .NotEmpty().WithMessage("Kategori ismi gereklidir.")
                .MustAsync(MustUniqueCategoryNameAsync).WithMessage("Kategori ismi veritabanında bulunmaktadır.");
        }

        private async Task<bool> MustUniqueCategoryNameAsync(string name, CancellationToken cancellationToken)
        {
            var anyCategory = await _categoryRepository.AnyAsync(x => x.Name == name);

            return !anyCategory;
        }
    }
}
