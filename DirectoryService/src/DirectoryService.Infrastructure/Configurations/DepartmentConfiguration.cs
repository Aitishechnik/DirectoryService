using DirectoryService.Domain.Entities.Departments;
using DirectoryService.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("departments");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .HasColumnName("id");

            builder.ComplexProperty(d => d.Name, nb =>
            {
                nb.Property(n => n.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_DEPARTMENT_NAME_LENGTH);
            });

            builder.ComplexProperty(d => d.Identifier, ib =>
            {
                ib.Property(i => i.Identifier)
                    .HasColumnName("identifier")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_IDENTIFIER_LENGTH);
            });

            builder.ComplexProperty(d => d.Path, pb =>
            {
                pb.Property(p => p.Path)
                    .HasColumnName("path")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_DEPARTMENT_NAME_LENGTH);
            });

            builder.Property(d => d.Depth)
                .HasColumnName("depth")
                .IsRequired();

            builder.HasMany(d => d.Children)
                .WithOne()
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(d => d.Locations)
                .WithMany(l => l.Departments)
                .UsingEntity(builder =>
                {
                    builder.ToTable("department_locations");
                    builder.Property<Guid>("DepartmentId")
                        .HasColumnName("department_id")
                        .IsRequired();
                    builder.Property<Guid>("LocationId")
                        .HasColumnName("location_id")
                        .IsRequired();
                });

            builder.HasMany(d => d.Positions)
                .WithMany(p => p.Departments)
                .UsingEntity(builder =>
                {
                    builder.ToTable("department_positions");
                    builder.Property<Guid>("DepartmentId")
                        .HasColumnName("department_id")
                        .IsRequired();
                    builder.Property<Guid>("PositionId")
                        .HasColumnName("position_id")
                        .IsRequired();
                });

            builder.Property(d => d.ChildrenCount)
                .HasColumnName("children_count")
                .IsRequired();

            builder.Property(d => d.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(d => d.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            builder.Property(d => d.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}