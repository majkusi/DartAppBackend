using DartAppClean.Domain.Entities.MatchEntites;

namespace DartAppClean.Application.Match.Queries.TeamQueries;
public class TeamsDto
{
    public int Id { get; init; }
    public int TeamNumber { get; init; }
    public int MatchId { get; init; }
    public int? Score { get; init; }

    public ICollection<TeamPlayer> Players { get; init; } = new List<TeamPlayer>();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Team, TeamsDto>()
                .ForMember(
                    dest => dest.Players,
                    opt => opt.MapFrom(src => src.Players.OrderBy(p => p.Order))
                );
        }
    }
}
