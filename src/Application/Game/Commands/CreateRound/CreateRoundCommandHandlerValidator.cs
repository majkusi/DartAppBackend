namespace DartAppClean.Application.Match.Commands.CreateRound;
public class CreateRoundCommandHandlerValidator : AbstractValidator<CreateRoundCommand>
{
    public CreateRoundCommandHandlerValidator()
    {
        RuleFor(c => c.Points)
            .NotEmpty()
            .LessThanOrEqualTo(180);
        RuleFor(c => c.PlayerUsername)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(10);
        RuleFor(c => c.GameId)
            .NotEmpty()
            .NotNull();
    }

}
