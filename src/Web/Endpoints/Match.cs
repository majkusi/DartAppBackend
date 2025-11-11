
using DartAppClean.Application.Match.Commands.CreateMatch;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DartAppClean.Web.Endpoints;

public class Match :EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateMatch).RequireAuthorization();
    }


    public async Task<Created<int>> CreateMatch(ISender sender, CreateMatchCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Match)}/{id}", id);
    }
}
