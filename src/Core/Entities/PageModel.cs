using System.Collections.Generic;

namespace Core.Entities
{
    public class PageModel<T> where T: class
    {
        public IEnumerable<T> Data { get; }
        public int TotalCount { get; }

        public PageModel(IEnumerable<T> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }
    }
}
