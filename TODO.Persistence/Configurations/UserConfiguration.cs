using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TODO.Domain.Users;

namespace TODO.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.ModifiedAt).IsRequired(false);

            builder.Property(i => i.Username)
                .HasMaxLength(50);

            builder.HasMany(i => i.ToDos)
                .WithOne(i => i.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasQueryFilter(i => (int)i.Status != 2);

        }
    }
}
