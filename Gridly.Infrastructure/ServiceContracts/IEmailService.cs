namespace Loot.Infrastructure.ServiceContracts;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}