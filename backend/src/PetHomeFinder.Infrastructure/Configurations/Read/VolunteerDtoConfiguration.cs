using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Domain.PetManagement.AggregateRoot;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Infrastructure.Configurations.Read;

public sealed class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(x => x.Id);

        builder.HasMany(p => p.Pets)
            .WithOne()
            .HasForeignKey(x => x.VolunteerId);
    }
}