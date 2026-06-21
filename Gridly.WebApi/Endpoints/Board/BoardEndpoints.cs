using Gridly.Application.Commands;

using MediatR;

namespace Loot.WebApi.Endpoints.Board;

public static class BoardEndpoints
{
    public static WebApplication RegisterBoardEndpoints(this WebApplication app)
    {
        // var group = app.MapGroup("boards");
        
        app.MapPost("/boards", async (CreateBoardCommand command, ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });
        
        return app;
    }
}