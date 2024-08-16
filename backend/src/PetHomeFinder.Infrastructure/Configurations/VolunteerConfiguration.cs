using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Domain;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Infrastructure.Configurations;

public sealed class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FirstName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(v => v.LastName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(v => v.Surname)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(v => v.Experience)
            .IsRequired();

        builder.Property(v => v.PetHomeFoundCount)
            .IsRequired();

        builder.Property(v => v.PetTreatmentCount)
            .IsRequired();

        builder.Property(v => v.PetSearchForHomeCount)
            .IsRequired();

        builder.Property(v => v.PhoneNumber)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.HasMany(v => v.Credentials)
            .WithOne();

        builder.HasMany(v => v.SocialNetworks)
            .WithMany();

        builder.HasMany(v => v.PetsOwning)
            .WithOne()
            .HasForeignKey("owner_id");

    }
}
