using Core.Entities;
using Core.Interfaces;
using Core.Services;
using LinqKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        public CommentService(ICommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            return await _repository.AddAsync(comment);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            await _repository.DeleteAsync(commentId);
        }

        public async Task<Comment> FetchCommentAsync(CommentSearchParams searchParams)
        {
            var predicate = PredicateBuilder.New<Comment>(true);

            if (!string.IsNullOrEmpty(searchParams.Author))
            {
                predicate = predicate.And((p) => p.User.Username == searchParams.Author || p.User.Email == searchParams.Author);
            }

            if (searchParams.CreatedOn.HasValue)
            {
                predicate = predicate.And((p) => p.CreatedAt == searchParams.CreatedOn.Value.ToUniversalTime());
            }

            return await _repository.FirstOrDefaultAsync(predicate);
        }

        public async Task<Comment> FetchCommentByIdAsync(int id)
        {
            return await _repository.FirstOrDefaultAsync((c) => c.Id == id);
        }

        public async Task<PageModel<Comment>> FetchCommentsAsync(CommentSearchParams searchParams)
        {
            var predicate = PredicateBuilder.New<Comment>(true);

            if (!string.IsNullOrEmpty(searchParams.Author))
            {
                predicate = predicate.And((c) => c.User.Username == searchParams.Author || c.User.Email == searchParams.Author);
            }

            if (searchParams.CreatedOn.HasValue)
            {
                predicate = predicate.And((u) => u.CreatedAt == searchParams.CreatedOn.Value.ToUniversalTime());
            }

            IEnumerable<Comment> comments = await _repository.GetAllAsync(predicate, searchParams.Page.Value, searchParams.Limit.Value);
            int totalCount = await _repository.CountAsync(predicate);
            return new PageModel<Comment>(comments, totalCount);
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            return await _repository.UpdateAsync(comment);
        }
    }
}
