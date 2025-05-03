using Curb.API.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curb.API.DataAccess.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Username);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.Username).HasColumnName("username");
        builder.Property(x => x.FirstName).HasColumnName("name");
        builder.Property(x => x.LastName).HasColumnName("last_name");
        builder.Property(x => x.PhotoUrl).HasColumnName("photo_url");
        builder.Property(x => x.AuthDate).HasColumnName("auth_date");
        builder.Property(x => x.CreeatedAt).HasColumnName("created_at");
    }
}
