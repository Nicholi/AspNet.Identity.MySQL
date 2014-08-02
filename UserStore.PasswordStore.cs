using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet.Identity.MySQL
{
    public partial class UserStore : IUserPasswordStore<IdentityUser>
    {
        /// <summary>
        /// Returns the PasswordHash for a given IdentityUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            string passwordHash = userTable.GetPasswordHash(user.Id);

            return Task.FromResult<string>(passwordHash);
        }

        /// <summary>
        /// Verifies if user has password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            var hasPassword = !string.IsNullOrEmpty(userTable.GetPasswordHash(user.Id));

            return Task.FromResult<bool>(hasPassword);
        }

        /// <summary>
        /// Sets the password hash for a given IdentityUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<Object>(null);
        }
    }
}
