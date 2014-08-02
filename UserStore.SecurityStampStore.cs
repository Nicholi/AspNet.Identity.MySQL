using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet.Identity.MySQL
{
    public partial class UserStore : IUserSecurityStampStore<IdentityUser>
    {
        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}
