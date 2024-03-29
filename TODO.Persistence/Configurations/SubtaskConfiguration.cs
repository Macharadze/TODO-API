using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TODO.Domain.Subtasks;

namespace TODO.Persistence.Configurations
{
    public class SubtaskConfiguration : IEntityTypeConfiguration<Subtask>
    {
        public void Configure(EntityTypeBuilder<Subtask> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.ModifiedAt).IsRequired(false);

            builder.Property(i=>i.Title).IsRequired().HasMaxLength(100);
            builder.HasOne(i => i.ToDo).WithMany(i => i.Subtasks);
            builder.HasQueryFilter(i => (int)i.Status != 2);
        }
    }
}
