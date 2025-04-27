using Curb.API.Contracts.Enums;

namespace Curb.API.Contracts.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; } = null!;
    public UserRole Role { get; set; }
}
