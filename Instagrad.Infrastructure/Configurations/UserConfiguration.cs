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
        //builder.Property(u => u.Images)
        //    .HasField("_images");

        builder.HasMany(u => u.Friends);
        //builder.Property(u => u.Friends)
        //    .HasField("_friends");

        builder.HasMany(u => u.IncomingFrendshipRequests)
            .WithMany(u => u.OutgoingFrendshipRepuests);
        //builder.Property(u => u.IncomingFrendshipRequests)
        //    .HasField("_incomingFrendshipRequests");

        builder.HasMany(u => u.OutgoingFrendshipRepuests)
            .WithMany(u => u.IncomingFrendshipRequests);
        //builder.Property(u => u.OutgoingFrendshipRepuests)
        //    .HasField("_outgoingFrendshipRequests");
        
    }
}