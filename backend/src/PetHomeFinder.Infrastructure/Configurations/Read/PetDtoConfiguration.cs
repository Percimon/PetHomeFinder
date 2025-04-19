using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Infrastructure.Configurations.Read;

public sealed class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(x => x.Id);
        
        builder.ComplexProperty(x => x.Address, tb =>
        {
            tb.Property(d => d.City)
                .HasColumnName("city");

            tb.Property(d => d.District)
                .HasColumnName("district");

            tb.Property(d => d.Street)
                .HasColumnName("street");

            tb.Property(d => d.Structure)
                .HasColumnName("structure");
        });
        
        builder.Property(v => v.Credentials)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<CredentialDto[]>(json, JsonSerializerOptions.Default)!);
        
        builder.Property(v => v.Photos)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<PetPhotoDto[]>(json, JsonSerializerOptions.Default)!);
    }
}