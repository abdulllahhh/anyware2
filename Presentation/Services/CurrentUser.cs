using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _context;
        public CurrentUser(IHttpContextAccessor httpContext)
        {
            _context = httpContext;
        }
        public Guid UserId
        {
            get
            {
                var userIdClaim = _context.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                return Guid.TryParse(userIdClaim, out var id)
                    ? id
                    : throw new AppException("Invalid or missing user id");
            }
        }
    }
}
