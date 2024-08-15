using System;
using PetHomeFinder.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetHomeFinder.Infrastructure.Configurations;

public sealed class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
{

    public void Configure(EntityTypeBuilder<PetPhoto> builder)
    {
        builder.ToTable("pet_photos");
        builder.HasKey(v => v.Id);
    }
}
