using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder
            .HasOne(a => a.User)
            .WithOne(u => u.VolunteerAccount)
            .HasForeignKey<VolunteerAccount>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(v => v.Credentials)
            .ValueObjectsCollectionJsonConversion(
                credential => new CredentialDto(credential.Name, credential.Description),
                dto => Credential.Create(dto.Name, dto.Description).Value)
            .HasColumnName("credentials");
    }
}