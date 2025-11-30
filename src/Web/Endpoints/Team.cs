using DartAppClean.Application.Match.Queries.TeamQueries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DartAppClean.Web.Endpoints;

public class Team : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetTeamsByMatchId, "{id}");
    }
    public async Task<Results<Ok<TeamsVm>, NotFound>> GetTeamsByMatchId(int id, ISender sender)
    {
        var teams = await sender.Send(new GetTeamsByMatchIdQuery(id));
        return TypedResults.Ok(teams);
    }
}
