namespace Curb.API.Contracts;

internal class ApiConfiguration(IConfiguration configuration)
{
    public string Issuer { get; private set; } = configuration["Jwt:Issuer"]!;
    public string Audience { get; private set; } = configuration["Jwt:Audience"]!;
    public string Key { get; private set; } = configuration["Jwt:Key"]!;
    public string BotToken { get; private set; } = configuration["BotConfiguration:BotToken"]!;
}
