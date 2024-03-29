using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TODO.Domain.Users;
using TODO.Persistence.Context;

namespace TODO.Persistence.Seed
{
    public static class UserSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ApplicationDBcontext>();

            database.Database.Migrate();

           // tu sachiro iqna userebis damateb //
            await database.SaveChangesAsync(true);

        }

    }
}
