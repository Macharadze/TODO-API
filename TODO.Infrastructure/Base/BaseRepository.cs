using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using TODO.Domain.Users;
using TODO.Persistence.Context;

namespace TODO.Infrastructure.Base
{
    public class BaseRepository<T> where T : class
    {
        protected readonly DbContext _context;

        protected readonly DbSet<T> _dbSet;
        protected IHttpContextAccessor _httpContextAccessor;
        public IQueryable<T> Table
        {
            get
            {
                return _dbSet;
            }
        }
        public BaseRepository(DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<T>> GetAllAsync(CancellationToken token)
        {
            return await _dbSet.ToListAsync(token);
        }
        public async Task<List<T>> GetAllAsync(CancellationToken token, Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> GetAsync(CancellationToken token, Expression<Func<T, bool>> predicate)
        {
            var tracked = await _dbSet.FirstOrDefaultAsync(predicate, token);
            return tracked;
        }

        public async Task AddAsync(CancellationToken token, T entity)
        {
            await _dbSet.AddAsync(entity, token);
            await _context.SaveChangesAsync(true, token);
        }

        public async Task UpdateAsync(CancellationToken token, T entity)
        {
            if (entity == null)
                return;

            _dbSet.Update(entity);
            await _context.SaveChangesAsync(true, token);
        }

        public async Task RemoveAsync(CancellationToken token, T entity)
        {
            if (entity == null)
                return;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(true, token);
        }

        public Task<bool> AnyAsync(CancellationToken token, Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate, token);
        }
        public async Task<string> GetAuthorizedId()
        {
            await Task.CompletedTask;
            var http = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (http is null)
                throw new InvalidOperationException("");

            return http.Value.ToString();
        }


    }
}
