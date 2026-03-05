using ContentService.Application.DTOs.Payloads;
using FluentValidation;

namespace ContentService.Application.Commands.CreateContent;

public class CreateContentValidator : AbstractValidator<CreateContentCommand>
{
    public CreateContentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.CreatedBy)
            .NotEqual(Guid.Empty).WithMessage("CreatedBy must be a valid non-empty GUID.");

        RuleFor(x => x.Payload)
            .NotNull().WithMessage("Payload cannot be null.");

        RuleFor(x => x.Payload)
            .Must(payload => payload is TextPayloadDto or MediaPayloadDto)
            .When(x => x.Payload is not null)
            .WithMessage("Payload type must be 'text' or 'media'.");

        RuleFor(x => (x.Payload as TextPayloadDto)!.Text)
            .NotEmpty().WithMessage("Text is required for text payload.")
            .When(x => x.Payload is TextPayloadDto);

        RuleFor(x => (x.Payload as MediaPayloadDto)!.Url)
            .NotEmpty().WithMessage("Url is required for media payload.")
            .When(x => x.Payload is MediaPayloadDto);
    }
}
