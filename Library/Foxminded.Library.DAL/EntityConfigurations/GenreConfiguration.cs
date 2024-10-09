using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Foxminded.Library.Domain.Entities;

namespace Foxminded.Library.DAL.EntitiesConfigurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder
            .HasIndex(g => new { g.Id })
            .IsUnique();
    }
}
