
using DartAppClean.Application.Match.Commands.CreateMatch;
using DartAppClean.Application.Match.Queries.GetMatchState;
using DartAppClean.Application.Match.Queries.MatchQueries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DartAppClean.Web.Endpoints;

public class Match : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateMatch);
        groupBuilder.MapGet(GetMatchById, "{id}");
        groupBuilder.MapGet(GetMatchState, "/matchstate/{id}");
    }


    public async Task<Created<int>> CreateMatch(ISender sender, CreateMatchCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Match)}/{id}", id);
    }

    public async Task<Results<Ok<MatchVm>, NotFound>> GetMatchById(int id, ISender sender)
    {
        var match = await sender.Send(new GetMatchByIdQuery(id));
        return TypedResults.Ok(match);
    }

    public async Task<Results<Ok<GetMatchStateResponse>, NotFound>> GetMatchState(int id,  ISender sender)
    {
        var matchState = await sender.Send(new GetMatchStateCommand(id));
        return TypedResults.Ok(matchState);
    }

}
