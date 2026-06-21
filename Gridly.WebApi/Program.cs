using FluentValidation;
using Gridly.Application.Commands;
using Gridly.Infrastructure.Consumers;
using Gridly.Infrastructure.DbContext;

using Gridly.WebApi;
using Gridly.WebApi.Endpoints.Board;
using Gridly.WebApi.Endpoints.User;
using Gridly.WebApi.ExceptionHandler;
using Scalar.AspNetCore;

using Serilog;

using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.ConfigureMediatr();
builder.AddMassTransitRabbitMq("loot-rabbitmq", massTransitConfiguration: cfg =>
{
    cfg.AddConsumer<ConfirmationEmailConsumer>();
});
builder.AddNpgsqlDbContext<LootDbContext>("loot-db");
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
builder.Services.ConfigureServices();
builder.Services.ConfigureOptions();
builder.Services.ConfigureSerilog(builder.Configuration);
builder.Services.ConfigureCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();
app.UseHsts();
app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterUserEndpoints();
app.RegisterBoardEndpoints();
app.Run();