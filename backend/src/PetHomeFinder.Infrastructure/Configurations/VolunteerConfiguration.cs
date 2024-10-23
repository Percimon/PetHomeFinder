using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.Volunteers;

namespace PetHomeFinder.Infrastructure.Configurations;

public sealed class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value)
            );

        builder.ComplexProperty(x => x.FullName, nb =>
        {
            nb.Property(d => d.FirstName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .HasColumnName("first_name");

            nb.Property(d => d.LastName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .HasColumnName("last_name");

            nb.Property(d => d.Surname)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .HasColumnName("surname");

        });

        builder.ComplexProperty(x => x.Description, tb =>
        {
            tb.Property(d => d.Value)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
            .HasColumnName("description");
        });

        builder.ComplexProperty(x => x.Experience, tb =>
        {
            tb.Property(d => d.Value)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .HasColumnName("experience");
        });

        builder.ComplexProperty(x => x.PhoneNumber, tb =>
        {
            tb.Property(d => d.Value)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .HasColumnName("phone_number");
        });

        builder.OwnsOne(v => v.Credentials, vb =>
        {
            vb.ToJson("credentials");

            vb.OwnsMany(cl => cl.Credentials, cl =>
            {
                cl.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("credential_name");

                cl.Property(c => c.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("credential_description"); ;
            });
        });

        builder.OwnsOne(v => v.SocialNetworks, vb =>
        {
            vb.ToJson("social_networks");

            vb.OwnsMany(cl => cl.SocialNetworks, cl =>
            {
                cl.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("social_network_name");


                cl.Property(c => c.Link)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("social_network_link");

            });
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        builder.HasMany(v => v.PetsOwning)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.NoAction);

    }
}
