using System;
using PetHomeFinder.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Infrastructure.Configurations;

public sealed class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
{

    public void Configure(EntityTypeBuilder<PetPhoto> builder)
    {
        builder.ToTable("pet_photos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FilePath)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.Property(p => p.IsMain)
                .IsRequired();
    }
}
