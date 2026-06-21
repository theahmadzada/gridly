using System.ComponentModel.DataAnnotations;

namespace Loot.Shared.Settings;

public record EmailSettings
{
    public const string SectionName = "EmailSettings";
    
    [Required(AllowEmptyStrings = false)]
    public string From { get; init; } =  string.Empty;
    
    [Required(AllowEmptyStrings = false)]
    public string Host { get; init; } =  string.Empty;
    
    public int Port { get; init; }
    
    public string? Username { get; init; }
    
    public string? Password { get; init; } 
}