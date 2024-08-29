using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Domain.Pets;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value)
            );

        builder.HasMany(v => v.BreedList)
           .WithOne()
           .HasForeignKey("species_id")
           .OnDelete(DeleteBehavior.Cascade);
    }
}
