using DartAppClean.Application.Common.Interfaces;

namespace DartAppClean.Application.Match.Queries.TeamQueries;

public record GetTeamsByMatchIdQuery(int Id) : IRequest<TeamsVm>;

public class GetTeamsByMatchIdQueryHandler : IRequestHandler<GetTeamsByMatchIdQuery, TeamsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTeamsByMatchIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TeamsVm> Handle(GetTeamsByMatchIdQuery request, CancellationToken token)
    {
        var teamsDto = await _context.Team
            .Where(team => team.GameId == request.Id)
            .AsNoTracking()
            .ProjectTo<TeamsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(token);

        return new TeamsVm
        {
            Teams = teamsDto ?? new List<TeamsDto>()
        };
    }
}
