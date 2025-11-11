
using DartAppClean.Application.Match.Commands.CreateTeam;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DartAppClean.Web.Endpoints;

public class Team :EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateTeam).RequireAuthorization();
    }

    public async Task<Created<int>> CreateTeam(ISender sender, CreateTeamCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Match)}/{id}", id);
    }
}
