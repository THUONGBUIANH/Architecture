using Architecture_BE.Models.Dto;
using FluentValidation;

namespace Architecture_BE.Models.Validations
{
    public class EditUserDtoValidator : AbstractValidator<EditUserDto>
    {
        public EditUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
