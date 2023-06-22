using Ordering.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
    public interface ITokenService
    {
        string CreateTokenAsync(AppUser user);
    }
}
