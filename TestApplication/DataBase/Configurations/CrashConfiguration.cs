using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestApplication.DataBase.Entities;

namespace TestApplication.DataBase.Configurations
{
    public class CrashConfiguration : IEntityTypeConfiguration<CrashEntity>
    {
        public void Configure(EntityTypeBuilder<CrashEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
           
            builder.HasMany(c => c.Operations)
                .WithOne()
                .HasForeignKey(o => o.CrashId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<StatusEntity>()
                 .WithMany()  
                 .HasForeignKey(x => x.StatusId)
                 .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
