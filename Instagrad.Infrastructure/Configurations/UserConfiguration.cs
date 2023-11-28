using Instagrad.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagrad.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Login);

        builder.Property(u => u.Login)
            .HasField("_login")
            .IsRequired();

        builder.Property("_password")
            .IsRequired();

        builder.HasMany(u => u.Images)
            .WithOne()
            .HasForeignKey(im => im.PublisherLogin);

        builder.HasMany(u => u.Friends);

        builder.HasMany(u => u.IncomingFriendshipRequests)
            .WithMany(u => u.OutgoingFriendshipRequests);

        builder.HasMany(u => u.OutgoingFriendshipRequests)
            .WithMany(u => u.IncomingFriendshipRequests);
    }
}