using Core.Entities;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services
{
    public class PaginationService
    {
        private readonly string _hostname;
        private readonly string _requestPath;
        public PaginationService(
            string hostname, 
            string requestPath
        )
        {
            _hostname = hostname;
            _requestPath = requestPath;
        }

        public PagedResponse<T> BuildPagedResponse<T>(PageModel<T> model, int page, int limit) where T : class
        {
            if (!model.Data.Any())
            {
                return new PagedResponse<T>(
                    model.TotalCount,
                    limit,
                    0,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    model.Data
               );
            }

            Uri baseUri = new(_hostname);
            Dictionary<string, string> queryParams = new();
            int firstPageNo = 1;
            queryParams["page"] = firstPageNo.ToString();
            queryParams["limit"] = limit.ToString();
            string firstPage = new Uri(baseUri, QueryHelpers.AddQueryString(_requestPath, queryParams)).AbsoluteUri;

            int pages = (int)Math.Ceiling(model.TotalCount / (double)limit);
            queryParams["page"] = pages.ToString();
            string lastPage = new Uri(baseUri, QueryHelpers.AddQueryString(_requestPath, queryParams)).AbsoluteUri;

            int prevPage = (page > firstPageNo) ? page - 1 : firstPageNo;
            queryParams["page"] = prevPage.ToString();
            string previousPage = new Uri(baseUri, QueryHelpers.AddQueryString(_requestPath, queryParams)).AbsoluteUri;

            int nextPageNo = (page < pages) ? page + 1 : pages;
            queryParams["page"] = nextPageNo.ToString();
            string nextPage = new Uri(baseUri, QueryHelpers.AddQueryString(_requestPath, queryParams)).AbsoluteUri;

            PagedResponse<T> response = new(
                model.TotalCount,
                limit,
                pages,
                firstPage,
                lastPage,
                previousPage,
                nextPage,
                model.Data
            );
            return response;
        }
    }
}
