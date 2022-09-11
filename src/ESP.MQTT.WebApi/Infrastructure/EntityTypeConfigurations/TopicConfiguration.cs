using ESP.MQTT.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESP.MQTT.WebApi.Infrastructure.EntityTypeConfigurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("topic");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.CreationDate)
            .HasColumnName("creationdate")
            .IsRequired();
        
        builder.Property(e => e.ChangeDate)
            .HasColumnName("changedate");
    }
}