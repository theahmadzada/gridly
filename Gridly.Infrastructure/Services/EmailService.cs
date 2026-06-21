using Loot.Infrastructure.ServiceContracts;
using Loot.Shared.Settings;

using MimeKit;
using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;

namespace Loot.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<EmailSettings>  _emailSettings;
    
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        var options = _emailSettings.Value;
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Loot Team", "loot@noreply.com"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(options.Host, options.Port, cancellationToken: cancellationToken);
        // await client.AuthenticateAsync(options.Username, options.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}