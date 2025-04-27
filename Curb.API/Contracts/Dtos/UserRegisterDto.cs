using Microsoft.AspNetCore.Mvc;

namespace Curb.API.Contracts.Dtos;

public class UserRegisterDto
{
    [FromQuery(Name = "id")]
    public string Id { get; set; } = null!;
    [FromQuery(Name = "username")]
    public string? Username { get; set; }
    [FromQuery(Name = "first_name")]
    public string? FirstName { get; set; }
    [FromQuery(Name = "last_name")]
    public string? LastName { get; set; }
    [FromQuery(Name = "auth_date")]
    public string AuthDate { get; set; } = null!;
    [FromQuery(Name = "photo_url")]
    public string? PhotoUrl { get; set; }
    [FromQuery(Name = "hash")]
    public string Hash { get; set; } = null!;
}
