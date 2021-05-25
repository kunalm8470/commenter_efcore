using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommenterContext _context;
        public CommentRepository(CommenterContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddAsync(Comment entity)
        {
            var ent = await _context.Set<Comment>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return ent.Entity;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<Comment>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await _context.Set<Comment>().CountAsync(predicate);
        }

        public async Task<Comment> DeleteAsync(int id)
        {
            Comment found = await _context.Set<Comment>().FirstOrDefaultAsync((u) => u.Id == id);

            if (found == default)
                return found;

            _context.Set<Comment>().Remove(found);
            await _context.SaveChangesAsync();
            return found;
        }

        public async Task<Comment> FirstOrDefaultAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await _context
                .Set<Comment>()
                .Include((c) => c.User)
                .Include((c) => c.Post)
                .ThenInclude((c) => c.User)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(int page, int limit)
        {
            return await _context.Set<Comment>()
                .Include((c) => c.User)
                .Include((c) => c.Post)
                .ThenInclude((c) => c.User)
                .OrderBy((c) => c.Id)
                .Skip(page)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(Expression<Func<Comment, bool>> predicate, int page, int limit)
        {
            int offset = (page - 1) * limit;

            return await _context.Set<Comment>()
                .Include((c) => c.User)
                .Include((c) => c.Post)
                .Where(predicate)
                .OrderBy((c) => c.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Comment> UpdateAsync(Comment entity)
        {
            Comment c = _context.Set<Comment>().Find(entity.Id);

            c.Body = entity.Body;
            c.UpdatedAt = entity.UpdatedAt;

            _context.Set<Comment>().Update(c);
            await _context.SaveChangesAsync();
            return c;
        }
    }
}
