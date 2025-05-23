using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.AnimalSpecies.Infrastructure.Configurations.Read;

public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(s => s.Id);
        
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey(b => b.SpeciesId);
    }
}