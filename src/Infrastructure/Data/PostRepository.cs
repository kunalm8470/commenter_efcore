using Core.Entities;
using Core.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly CommenterContext _context;
        public PostRepository(CommenterContext context)
        {
            _context = context;
        }

        public async Task<Post> AddAsync(Post entity)
        {
            var ent = await _context.Set<Post>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return ent.Entity;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<Post>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Post, bool>> predicate)
        {
            return await _context.Set<Post>().CountAsync(predicate);
        }

        public async Task<Post> DeleteAsync(int id)
        {
            Post found = await _context.Set<Post>().FirstOrDefaultAsync((u) => u.Id == id);

            if (found == default)
                return found;

            _context.Set<Post>().Remove(found);
            await _context.SaveChangesAsync();
            return found;
        }

        public async Task<Post> FirstOrDefaultAsync(Expression<Func<Post, bool>> predicate)
        {
            return await _context.Set<Post>()
                .Include((p) => p.User)
                .Include((p) => p.Comments)
                .ThenInclude((p) => p.User)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Post>> GetAllAsync(int page, int limit)
        {
            return await _context.Set<Post>()
                .Include((p) => p.User)
                .Include((p) => p.Comments)
                .ThenInclude((p) => p.User)
                .OrderBy((p) => p.Id)
                .Skip(page)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllAsync(Expression<Func<Post, bool>> predicate, int page, int limit)
        {
            int offset = (page - 1) * limit;

            return await _context.Set<Post>()
                .Include((p) => p.User)
                .Include((p) => p.Comments)
                .ThenInclude((p) => p.User)
                .Where(predicate)
                .OrderBy((p) => p.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Post> UpdateAsync(Post entity)
        {
            Post p = _context.Set<Post>().Find(entity.Id);

            p.Title = entity.Title;
            p.Body = entity.Body;
            p.UpdatedAt = entity.UpdatedAt;

            _context.Set<Post>().Update(p);
            await _context.SaveChangesAsync();
            return p;
        }
    }
}
