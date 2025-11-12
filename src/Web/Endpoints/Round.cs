
using DartAppClean.Application.Match.Commands.CreateRound;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DartAppClean.Web.Endpoints;

public class Round :EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateRound).RequireAuthorization();
    }

    public async Task<Created<int>> CreateRound(ISender sender,CreateRoundCommand command, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Match)}/{id}", id);
    }
}
