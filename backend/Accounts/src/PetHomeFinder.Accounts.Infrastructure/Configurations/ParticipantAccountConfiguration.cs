using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Infrastructure.Configurations;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder
            .HasOne(a => a.User)
            .WithOne(u => u.ParticipantAccount)
            .HasForeignKey<ParticipantAccount>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}