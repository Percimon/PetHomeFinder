using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Accounts.Infrastructure.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder
            .HasOne(a => a.User)
            .WithOne(u => u.AdminAccount)
            .HasForeignKey<AdminAccount>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ComplexProperty(a => a.FullName, fb =>
        {
            fb.Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("first_name");

            fb.Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("last_name");

            fb.Property(d => d.Surname)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("surname");
        });
    }
}