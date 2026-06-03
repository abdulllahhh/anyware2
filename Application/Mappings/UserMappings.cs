using Application.DTOs.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public static class UserMappings
    {
        public static readonly Expression<Func<User, AdminUserDto>> ToAdminDto =
    user => new AdminUserDto
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
