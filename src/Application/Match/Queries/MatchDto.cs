using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Enums;

namespace DartAppClean.Application.Match.Queries;
public class MatchDto
{
    public int Id { get; init; }
    public X01TypeEnum? X01TypeEnum { get; init; }
    public CricketTypeEnum? CricketTypeEnum { get; init; }
    public ICollection<Team>? Teams { get; init; } = new List<Team>();
    public ICollection<Round>? Rounds { get; init; } = new List<Round>();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Game, MatchDto>();
        }
    }
}
