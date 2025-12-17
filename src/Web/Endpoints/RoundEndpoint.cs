
using DartAppClean.Application.Match.Commands.CreateRound;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DartAppClean.Web.Endpoints;

public class RoundEndpoint : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateRound);
    }

    public async Task<Created<int>> CreateRound(ISender sender, CreateRoundCommand command, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(MatchEndpoint)}/{id}", id);
    }

}
