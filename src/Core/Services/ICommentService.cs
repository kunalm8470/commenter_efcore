using Core.Entities;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ICommentService
    {
        public Task<PageModel<Comment>> FetchCommentsAsync(CommentSearchParams searchParams);
        public Task<Comment> FetchCommentAsync(CommentSearchParams searchParams);
        public Task<Comment> FetchCommentByIdAsync(int id);
        public Task<Comment> AddCommentAsync(Comment comment);
        public Task<Comment> UpdateCommentAsync(Comment comment);
        public Task DeleteCommentAsync(int commentId);
    }
}
