namespace Curb.API.DataAccess.Domains;

internal class User : BaseEntity
{
    public override Guid Id { get; set; }
    public long UserId { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime AuthDate { get; set; }
    public DateTime CreeatedAt { get; set; }
    public virtual Session? Session { get; set; }
}
