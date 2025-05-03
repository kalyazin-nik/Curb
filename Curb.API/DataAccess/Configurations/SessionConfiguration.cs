using Curb.API.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curb.API.DataAccess.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("sessions");

            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.RefreshToken).HasColumnName("refresh_token");
            builder.Property(x => x.ExpiryTime).HasColumnName("expiry_time");
            builder.Property(x => x.ForceLogout).HasColumnName("force_logout");
        }
    }
}
