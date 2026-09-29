namespace Warehouse.Auth;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "warehouse";
    public string Audience { get; set; } = "warehouse";
}
