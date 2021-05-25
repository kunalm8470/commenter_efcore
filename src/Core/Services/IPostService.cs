using Core.Entities;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IPostService
    {
        public Task<PageModel<Post>> FetchPostsAsync(PostSearchParams searchParams);
        public Task<Post> FetchPostAsync(PostSearchParams searchParams);
        public Task<Post> FetchPostByIdAsync(int id);
        public Task<Post> AddPostAsync(Post post);
        public Task<Post> UpdatePostAsync(Post post);
        public Task DeletePostAsync(int postId);
    }
}
