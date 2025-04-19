using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Domain.PetManagement.Entities;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;
using PetHomeFinder.Infrastructure.Extensions;

namespace PetHomeFinder.Infrastructure.Configurations.Write;

public sealed class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value)
            );

        builder.ComplexProperty(x => x.Name, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });

        builder.ComplexProperty(p => p.Position, sb =>
        {
            sb.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("position");
        });

        builder.ComplexProperty(p => p.SpeciesBreed, pb =>
        {
            pb.Property(s => s.SpeciesId)
                .HasConversion(
                    s => s.Value,
                    v => SpeciesId.Create(v))
                .IsRequired()
                .HasColumnName("species_id");

            pb.Property(b => b.BreedId)
                .IsRequired()
                .HasColumnName("breed_id");
        });

        builder.ComplexProperty(x => x.Description, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description");
        });

        builder.ComplexProperty(x => x.Color, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("color");
        });

        builder.ComplexProperty(x => x.HealthInfo, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("health_info");
        });

        builder.ComplexProperty(x => x.Address, tb =>
        {
            tb.Property(d => d.City)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("city");

            tb.Property(d => d.District)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("district");

            tb.Property(d => d.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("street");

            tb.Property(d => d.Structure)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("structure");
        });

        builder.ComplexProperty(x => x.Weight, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasColumnName("weight");
        });

        builder.ComplexProperty(x => x.Height, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasColumnName("height");
        });

        builder.ComplexProperty(x => x.OwnerPhoneNumber, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("owner_phone_number");
        });

        builder.Property(p => p.IsCastrated)
            .IsRequired()
            .HasColumnName("is_castrated");

        builder.Property(p => p.IsVaccinated)
            .IsRequired()
            .HasColumnName("is_vaccinated");

        builder.Property(p => p.BirthDate)
            .IsRequired()
            .HasColumnName("birth_date");

        builder.Property(p => p.CreateDate)
            .IsRequired()
            .HasColumnName("create_date");
        
        builder.Property(p => p.HelpStatus)
            .HasConversion(
                status => status.ToString(),
                value => (HelpStatusEnum)Enum.Parse(typeof(HelpStatusEnum), value!))
            .IsRequired()
            .HasColumnName("help_status");

        builder.Property(v => v.Credentials)
            .ValueObjectsCollectionJsonConversion(
                credential => new CredentialDto(credential.Description, credential.Name),
                dto => Credential.Create(dto.Name, dto.Description).Value)
            .HasColumnName("credentials");
        
        builder.Property(v => v.Photos)
            .ValueObjectsCollectionJsonConversion(
                petPhoto => new PetPhotoDto(petPhoto.FilePath, petPhoto.IsMain),
                dto => PetPhoto.Create(dto.PathToStorage, dto.IsMain).Value)
            .HasColumnName("photos");
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
        
    }
}