using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("locations");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                .HasColumnName("id");

            builder.ComplexProperty(l => l.Name, nb =>
            {
                nb.Property(n => n.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOCATION_NAME_LENGTH);
            });

            builder.ComplexProperty(l => l.Address, ab =>
            {
                ab.Property(a => a.State)
                    .HasColumnName("state")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_STATE_LENGTH);

                ab.Property(a => a.City)
                    .HasColumnName("city")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_CITY_LENGTH);

                ab.Property(a => a.Address)
                    .HasColumnName("address")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_ADDRESS_LENGTH);
            });

            builder.ComplexProperty(l => l.Timezone, tb =>
            {
                tb.Property(t => t.TimeZone)
                    .HasColumnName("timezone")
                    .IsRequired();
            });

            builder.Property(l => l.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(l => l.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            builder.Property(l => l.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnUpdate();
        }
    }
}