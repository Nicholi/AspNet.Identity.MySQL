using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet.Identity.MySQL
{
    public partial class UserStore : IUserLockoutStore<IdentityUser, string>
    {
        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}
