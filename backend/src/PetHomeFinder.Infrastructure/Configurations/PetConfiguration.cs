using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Domain;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Infrastructure.Configurations;

public sealed class PetConfiguration : IEntityTypeConfiguration<Pet>
{

        public void Configure(EntityTypeBuilder<Pet> builder)
        {
                builder.ToTable("pets");

                builder.HasKey(p => p.Id);

                builder.Property(p => p.Name)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                builder.Property(p => p.Species)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                builder.Property(p => p.Breed)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                builder.Property(p => p.Color)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                builder.Property(p => p.HealthInfo)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

                builder.Property(p => p.Address)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                builder.Property(p => p.Weight)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                builder.Property(p => p.Height)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                builder.Property(p => p.OwnerPhoneNumber)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                builder.Property(p => p.IsCastrated)
                        .IsRequired();

                builder.Property(p => p.IsVaccinated)
                        .IsRequired();

                builder.Property(p => p.HelpStatus)
                       .IsRequired();

                builder.HasMany(p => p.Photos)
                        .WithOne()
                        .HasForeignKey("photo_id");

                builder.HasMany(p => p.Credentials)
                        .WithOne();
        }
}