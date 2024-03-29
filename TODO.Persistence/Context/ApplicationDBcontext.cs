using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TODO.Domain.ActionResult;
using TODO.Domain.Subtasks;
using TODO.Domain.TODO;
using TODO.Domain.Users;
using TODO.Persistence.Entry;

namespace TODO.Persistence.Context
{
    public class ApplicationDBcontext : DbContext
    {
        public ApplicationDBcontext(DbContextOptions<ApplicationDBcontext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> TODOs { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBcontext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess);
            await OnAfterSaveChanges(auditEntries);
            var result2 = await base.SaveChangesAsync(acceptAllChangesOnSuccess);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry is ActionLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditentry = new AuditEntry(entry);
                auditentry.TableName = entry.Metadata.GetTableName();
                auditEntries.Add(auditentry);

                foreach (var item in entry.Properties)
                {
                    if (item.IsTemporary)
                    {
                        auditentry.TemporaryProperties.Add(item);
                        continue;
                    }
                    string name = item.Metadata.Name;
                    if (item.Metadata.IsPrimaryKey())
                    {
                        auditentry.KeyValues[name] = item.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditentry.NewValues[name] = item.CurrentValue;
                            auditentry.Status = Domain.Enums.Status.Created;
                            break;

                        case EntityState.Deleted:
                            auditentry.OldValues[name] = item.OriginalValue;
                            auditentry.Status = Domain.Enums.Status.Deleted;
                            break;

                        case EntityState.Modified:
                            if (item.IsModified)
                            {
                                auditentry.OldValues[name] = item.OriginalValue;
                                auditentry.NewValues[name] = item.CurrentValue;
                                auditentry.Status = Domain.Enums.Status.Updated;

                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
                ActionLogs.Add(auditEntry.ToAudit());

            return auditEntries;
        }


        private  async Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
            {
             await Task.CompletedTask;
                return;
            }

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                ActionLogs.Add(auditEntry.ToAudit());
            }
            await Task.CompletedTask;
        }
    }
}
