using DartAppClean.Application.Common.Models;

namespace DartAppClean.Application.Match.Queries;
public class MatchVm
{
    public MatchDto Match { get; init; } = default!;
    public IReadOnlyCollection<RoundDto> Rounds { get; init; } = Array.Empty<RoundDto>();
    public IReadOnlyCollection<LookupDto> StatusOptions { get; init; } = Array.Empty<LookupDto>();
}
