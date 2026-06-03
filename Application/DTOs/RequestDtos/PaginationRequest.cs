using Application.DTOs.User;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RequestDtos
{
    public class PaginationRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public UserSortBy? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public UserFilter? Filter { get; set; }
    }
    public class UserFilter
    {
        public UserRole? Role { get; set; }
        public string? SearchTerm { get; set; }
    }
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
    public class UsersPagedResult
    {
        public PagedResult<AdminUserDto> ActiveUsers { get; set; }
        public PagedResult<AdminUserDto> DeletedUsers { get; set; }
    }
}
