using DartAppClean.Application.Common.Interfaces;

namespace DartAppClean.Application.Match.Commands.CreateRound;
public class CreateRoundValidator : AbstractValidator<CreateRoundCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateRoundValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(c => c.Points)
            .NotEmpty()
            .LessThanOrEqualTo(180);
        RuleFor(c => c.PlayerUsername)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(10);

    }

}
