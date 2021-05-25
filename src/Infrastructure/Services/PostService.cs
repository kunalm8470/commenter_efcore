using Core.Entities;
using Core.Interfaces;
using Core.Services;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;
        public PostService(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<Post> AddPostAsync(Post post)
        {
            return await _repository.AddAsync(post);
        }

        public async Task DeletePostAsync(int postId)
        {
            await _repository.DeleteAsync(postId);
        }

        public async Task<Post> FetchPostAsync(PostSearchParams searchParams)
        {
            var predicate = PredicateBuilder.New<Post>(true);

            if (!string.IsNullOrEmpty(searchParams.Author))
            {
                predicate = predicate.And((p) => p.User.Username == searchParams.Author && p.User.Email == searchParams.Author);
            }

            if (searchParams.CreatedOn.HasValue)
            {
                predicate = predicate.And((p) => p.CreatedAt == searchParams.CreatedOn.Value.ToUniversalTime());
            }

            return await _repository.FirstOrDefaultAsync(predicate);
        }

        public async Task<Post> FetchPostByIdAsync(int id)
        {
            return await _repository.FirstOrDefaultAsync((p) => p.Id == id);
        }

        public async Task<PageModel<Post>> FetchPostsAsync(PostSearchParams searchParams)
        {
            var predicate = PredicateBuilder.New<Post>(true);

            if (!string.IsNullOrEmpty(searchParams.Author))
            {
                predicate = predicate.And((p) => p.User.Username == searchParams.Author || p.User.Email == searchParams.Author);
            }

            if (searchParams.CreatedOn.HasValue)
            {
                DateTime yesterday = searchParams.CreatedOn.Value.AddDays(-1).ToUniversalTime();
                DateTime tomorrow = searchParams.CreatedOn.Value.AddDays(1).ToUniversalTime();

                predicate = predicate.And((p) => p.CreatedAt >= yesterday && p.CreatedAt <= tomorrow);
            }

            IEnumerable<Post> posts = await _repository.GetAllAsync(predicate, searchParams.Page.Value, searchParams.Limit.Value);
            int totalCount = await _repository.CountAsync(predicate);
            return new PageModel<Post>(posts, totalCount);
        }

        public async Task<Post> UpdatePostAsync(Post post)
        {
            return await _repository.UpdateAsync(post);
        }
    }
}
