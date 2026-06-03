using Application.Exceptions;
using Application.Interfaces;
using System.Security.Claims;

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
                    : throw new AppException("User Not Logged In or Not Found");
            }
        }
    }
}
