using Instagrad.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagrad.Infrastructure.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(im => im.Id);
        builder.Property(u => u.PublisherLogin);

        //builder.HasOne(im => im.PublisherLogin)
        //    .WithMany(u => u.Images);       
    }
}