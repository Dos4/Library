using Foxminded.Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foxminded.Library.DAL.EntityConfigurations;

public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder
            .HasIndex(p => new { p.Id })
            .IsUnique();
    }
}
