using DartAppClean.Application.Match.Commands.CreateMatch;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DartAppClean.Web.Endpoints;

public class Game : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateGame);

    }


    public async Task<Created<int>> CreateGame(ISender sender, CreateMatchCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Game)}/{id}", id);
    }



}
