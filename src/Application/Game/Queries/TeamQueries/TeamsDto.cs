using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Application.Match.Queries.TeamQueries;
public class TeamsDto
{
    public int Id { get; init; }
    public int TeamNumber { get; init; }
    public int GameId { get; init; }
    public int? Score { get; init; }

    public ICollection<TeamPlayer> Players { get; init; } = new List<TeamPlayer>();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Team, TeamsDto>();
        }
    }
}
