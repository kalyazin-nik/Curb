namespace Curb.API.DataAccess.Domains;

internal class User : BaseEntity
{
    public override Guid Id { get; set; }
    public long UserId { get; set; }
    public string? Username { get; set; } = null!;
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? PhotoUrl { get; set; } = null!;
    public DateTime AuthDate { get; set; }
    public DateTime CreeatedAt { get; set; }
    public virtual Session? Session { get; set; }
}
