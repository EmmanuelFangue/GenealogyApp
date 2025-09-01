using FluentValidation;
using static GenealogyApp.API.Endpoints.PeopleEndpoints;

namespace GenealogyApp.API.Validators;

public class PersonCreateDtoValidator : AbstractValidator<PersonCreateDto>
{
    public PersonCreateDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).MaximumLength(100);
        RuleFor(x => x.Gender).Must(g => string.IsNullOrEmpty(g) || new[] { "M", "F", "U" }.Contains(g))
                              .WithMessage("Le genre doit être M/F/U ou vide.");
    }
}
