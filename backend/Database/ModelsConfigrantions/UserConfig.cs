using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.ModelsConfigrantions;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.id);

        builder.Property(u => u.id).ValueGeneratedOnAdd().IsRequired();

        builder.Property(u => u.Username).HasColumnType("varchar(200)").IsRequired();

        builder.Property(u => u.Email).HasColumnType("varchar(200)").IsRequired();

        builder.Property(u => u.Password).HasColumnType("varchar(200)").IsRequired();

        builder.Property(u => u.FirstName).HasColumnType("varchar(200)").IsRequired();

        builder.Property(u => u.LastName).HasColumnType("varchar(200)").IsRequired();
    }
}