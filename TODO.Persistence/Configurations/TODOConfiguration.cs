using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TODO.Domain.TODO;

namespace TODO.Persistence.Configurations
{
    public class TODOConfiguration : IEntityTypeConfiguration<ToDo>
    {
        public void Configure(EntityTypeBuilder<ToDo> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i=>i.Title).IsRequired().HasMaxLength(100);
            builder.Property(i => i.TargetCompletionDate).IsRequired(false);
            builder.Property(i => i.ModifiedAt).IsRequired(false);
            builder.HasQueryFilter(i => (int)i.Status != 2);

        }
    }
}
