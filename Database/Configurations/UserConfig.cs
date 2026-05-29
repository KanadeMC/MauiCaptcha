using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZALUPA.Database.Classes;

namespace ZALUPA.Database.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Login)
            .IsRequired()
            .HasColumnName("Login")
            .HasColumnType("varchar")
            .HasMaxLength(30);
        
        builder.Property(x => x.Password)
            .IsRequired()
            .HasColumnName("Password")
            .HasColumnType("varchar")
            .HasMaxLength(30);
    }
}