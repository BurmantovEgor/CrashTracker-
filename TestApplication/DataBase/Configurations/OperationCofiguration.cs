using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestApplication.DataBase.Entities;

namespace TestApplication.DataBase.Configurations
{
    public class OperationCofiguration : IEntityTypeConfiguration<OperationEntity>
    {
 
        public void Configure(EntityTypeBuilder<OperationEntity> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasOne<CrashEntity>()
              .WithMany(c => c.Operations)
              .HasForeignKey(o => o.CrashId)
              .OnDelete(DeleteBehavior.Cascade); // Удален
        }
    }
}
