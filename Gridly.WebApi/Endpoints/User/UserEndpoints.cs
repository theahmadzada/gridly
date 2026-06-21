using Gridly.Application.Commands;

using MediatR;

namespace Gridly.WebApi.Endpoints.User;

public static class UserEndpoints
{
    public static WebApplication RegisterUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users");
        
        group.MapPost("register", async (CreateUserCommand command, ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });

        group.MapPost("login", async (LogInUserCommand command, ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });
        
        group.MapPost("email", async (ConfirmEmailCommand command, ISender meadiator, CancellationToken cancellationToken) =>
        {
            var result = await meadiator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });
        
        return app;
    }
}