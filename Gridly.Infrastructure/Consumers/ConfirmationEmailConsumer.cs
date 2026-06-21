using System.Net;

using Loot.Infrastructure.ServiceContracts;
using Loot.Shared.Events;
using MassTransit;

namespace Loot.Infrastructure.Consumers;

public class ConfirmationEmailConsumer : IConsumer<UserCreatedEvent>
{ 
    private readonly IEmailService _emailService;
    
    public ConfirmationEmailConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var confirmationLink = $"http://localhost:5173/confirm-email?userId={context.Message.Id}&token={WebUtility.UrlEncode(context.Message.Token)}";
        var body = $"Please press the following link to confirm your email: {confirmationLink}";
        await _emailService.SendAsync(context.Message.Email, "Confirm your email", body);
    }
}