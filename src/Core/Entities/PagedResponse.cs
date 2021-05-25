using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class PagedResponse<T> where T : class
    {
        [JsonPropertyName("total")]
        public int TotalCount { get; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; }

        [JsonPropertyName("pages")]
        public int Pages { get; }

        [JsonPropertyName("first_page")]
        public string FirstPage { get; }

        [JsonPropertyName("last_page")]
        public string LastPage { get; }

        [JsonPropertyName("prev_page")]
        public string PreviousPage { get; }

        [JsonPropertyName("next_page")]
        public string NextPage { get; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; }

        public PagedResponse(
            int totalCount,
            int pageSize,
            int pages,
            string firstPage,
            string lastPage,
            string prevPage,
            string nextPage,
            IEnumerable<T> data
        )
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            Pages = pages;
            FirstPage = firstPage;
            LastPage = lastPage;
            PreviousPage = prevPage;
            NextPage = nextPage;
            Data = data;
        }
    }
}
