using System.ComponentModel.DataAnnotations;

namespace Loot.Shared.Settings;

public record RefreshTokenSettings()
{
    public const string SectionName = "JwtSettings";
    
    [Required(AllowEmptyStrings = false)]
    public string Key { get; init; }  = string.Empty;
    
    public int ValidFor { get; init; }
}