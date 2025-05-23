using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.AnimalSpecies.Domain.Entities;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;

namespace PetHomeFinder.AnimalSpecies.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value)
            );

        builder.ComplexProperty(x => x.Name, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });
    }
}