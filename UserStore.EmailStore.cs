using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet.Identity.MySQL
{
    public partial class UserStore : IUserEmailStore<IdentityUser>
    {
        public Task SetEmailAsync(IdentityUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
