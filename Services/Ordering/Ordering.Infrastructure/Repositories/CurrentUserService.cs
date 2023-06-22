using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public interface ICurrentUserService
    {
        string EmailUser { get; }
    }
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string EmailUser => (_httpContextAccessor.HttpContext == null ||
                                 _httpContextAccessor.HttpContext.User == null ||
                                 !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) ?
                                  "" :
                                    _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
    }
}
