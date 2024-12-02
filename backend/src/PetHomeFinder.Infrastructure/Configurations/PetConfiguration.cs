using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHomeFinder.Domain.PetManagement.Entities;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Infrastructure.Configurations;

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
                        .IsRequired()
                        .HasColumnName("help_status");

                builder.OwnsOne(p => p.Credentials, cb =>
                {
                        cb.ToJson("credentials");

                        cb.OwnsMany(cl => cl.Credentials, cl =>
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

                builder.OwnsOne(p => p.Photos, pb =>
                {
                        pb.ToJson("photos");

                        pb.OwnsMany(cl => cl.PetPhotos, ph =>
                        {
                                ph.Property(c => c.FilePath)
                                .IsRequired()
                                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                                .HasColumnName("file_path");

                                ph.Property(c => c.IsMain)
                                .IsRequired()
                                .HasColumnName("is_main"); ;
                        });
                });

        }
}