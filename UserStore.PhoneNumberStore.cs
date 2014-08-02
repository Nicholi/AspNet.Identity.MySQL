using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet.Identity.MySQL
{
    public partial class UserStore : IUserPhoneNumberStore<IdentityUser>
    {
        public Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }
    }
}
