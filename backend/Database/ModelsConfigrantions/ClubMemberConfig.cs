using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.ModelsConfigrantions;

public class ClubMemberConfig : IEntityTypeConfiguration<ClubMember>
{
    public void Configure(EntityTypeBuilder<ClubMember> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedOnAdd().IsRequired();

        builder.Property(c => c.CreatedAt).HasColumnType("datetime");

        builder.Property(c => c.DeletedAt).HasColumnType("datetime");

        builder.Property(c => c.IsModerator).HasColumnType("bit");

        builder.Property(c => c.IsOwner).HasColumnType("bit");


        // Relations
        builder.HasOne(m => m.User)
            .WithMany(u => u.ClubMember)
            .HasForeignKey(m => m.UserId)
            .IsRequired();

        builder.HasIndex(m => m.UserId).IsUnique();

        builder.HasOne(cm => cm.Club)
            .WithMany(c => c.ClubMembers)
            .HasForeignKey(m => m.ClubId)
            .IsRequired();

        builder.HasIndex(m => m.ClubId).IsUnique();
    }
}