using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestApplication.DataBase.Entities;

namespace TestApplication.DataBase.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {


        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(30);
            builder.Property(x => x.UserEmail).IsRequired().HasMaxLength(75);
        }

    }
}
