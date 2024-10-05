using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.ModelsConfigrantions;

public class ClubConfig : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.HasKey(c => c.id);

        builder.Property(c => c.id).ValueGeneratedOnAdd().IsRequired();

        builder.Property(c => c.IsPublic).HasColumnType("bit").IsRequired();

        builder.Property(c => c.Name)
            .HasColumnType("VARCHAR(200)")
            .IsRequired();

        builder.Property(c => c.Desciption)
            .HasColumnType("VARCHAR(500)");
    }
}