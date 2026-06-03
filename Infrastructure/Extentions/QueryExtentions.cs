using Application.DTOs.RequestDtos;
using Application.DTOs.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extentions
{
    public static class QueryExtentions
    {
        public static IQueryable<User> ApplyFilters(this IQueryable<User> query, PaginationRequest request)
        {
            if (request.Filter?.Role != null)
                query = query.Where(u => u.Role == request.Filter.Role);

            if (!string.IsNullOrEmpty(request.Filter?.SearchTerm))
            {
                var term = request.Filter.SearchTerm;
                query = query.Where(u => u.Email.Contains(term) || u.Name.Contains(term));
            }

            return query;
        }
        public static IQueryable<User> ApplySorting(this IQueryable<User> query, PaginationRequest request)
        {
            return request.SortBy switch
            {
                UserSortBy.Email => request.SortDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                UserSortBy.Name => request.SortDescending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                UserSortBy.CreatedAt => request.SortDescending ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };
        }
        public static IQueryable<User> ApplyPaging(this IQueryable<User> query, PaginationRequest request)
        {
            return query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);
        }
    }

}
