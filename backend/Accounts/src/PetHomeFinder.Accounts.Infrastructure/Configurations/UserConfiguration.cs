using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Accounts.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();
        
        builder.Property(v => v.SocialNetworks)
            .ValueObjectsCollectionJsonConversion(
                socialNetworks => new SocialNetworkDto(socialNetworks.Name, socialNetworks.Link),
                dto => SocialNetwork.Create(dto.Name, dto.Link).Value)
            .HasColumnName("social_networks");
    }
}