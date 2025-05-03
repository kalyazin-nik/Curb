namespace Curb.API.DataAccess.Domains;

internal class Session : BaseEntity
{
    public override Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiryTime { get; set; }
    public bool ForceLogout { get; set; } = false;
    public virtual User? User { get; set; }
}
