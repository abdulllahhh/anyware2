using Application.DTOs.RequestDtos;
using Application.DTOs.User;
using Application.Interfaces.Query;
using Application.Mappings;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services
{
    public class UserQueryService : IUserQueryService
    {
        private readonly AppDbContext _context;
        public UserQueryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UsersPagedResult> GetPagedAsync(PaginationRequest request)
        {
            var baseQuery = _context.Users.AsNoTracking();

            // apply filters once
            if (request.Filter?.Role != null)
                baseQuery = baseQuery.Where(u => u.Role == request.Filter.Role);

            if (!string.IsNullOrEmpty(request.Filter?.SearchTerm))
            {
                var term = request.Filter.SearchTerm;
                baseQuery = baseQuery.Where(u =>
                    u.Email.Contains(term) || u.Name.Contains(term));
            }

            // split queries
            var activeQuery = baseQuery.Where(u => !u.IsDeleted);
            var deletedQuery = baseQuery.Where(u => u.IsDeleted);

            // counts (cheap queries)
            var activeCountTask = activeQuery.CountAsync();
            var deletedCountTask = deletedQuery.CountAsync();

            // paging (same rules applied)
            var activeItemsTask = await activeQuery
                .ApplySorting(request)
                .ApplyPaging(request)
                .Select(UserMappings.ToAdminDto)
                .ToListAsync();

            var deletedItemsTask = await deletedQuery
                .ApplySorting(request)
                .ApplyPaging(request)
                .Select(UserMappings.ToAdminDto)
                .ToListAsync();

            await Task.WhenAll(activeCountTask, deletedCountTask, activeItemsTask, deletedItemsTask);

            return new UsersPagedResult
            {
                ActiveUsers = new PagedResult<AdminUserDto>
                {
                    Items = await activeItemsTask,
                    TotalCount = await activeCountTask,
                    Page = request.Page,
                    PageSize = request.PageSize
                },
                DeletedUsers = new PagedResult<AdminUserDto>
                {
                    Items = await deletedItemsTask,
                    TotalCount = await deletedCountTask,
                    Page = request.Page,
                    PageSize = request.PageSize
                }
            };
        }

        public AdminUserDto MapToDto(User user)
        {
            return new AdminUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsDeleted = user.IsDeleted,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
