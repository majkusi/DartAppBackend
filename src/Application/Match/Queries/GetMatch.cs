using DartAppClean.Application.Common.Interfaces;

namespace DartAppClean.Application.Match.Queries;

public record GetMatchByIdQuery(int Id) : IRequest<MatchVm>;

public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, MatchVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetMatchByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<MatchVm> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
    {
        var matchDto = await _context.Game
            .Where(g => g.Id == request.Id)
            .AsNoTracking()
            .ProjectTo<MatchDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return new MatchVm
        {
            Match = matchDto ?? new MatchDto(),
        };
    }
}
