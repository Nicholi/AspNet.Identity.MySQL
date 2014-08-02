using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet.Identity.MySQL
{
    public partial class UserStore : IUserClaimStore<IdentityUser>
    {
        /// <summary>
        /// Inserts a claim to the UserClaimsTable for the given user
        /// </summary>
        /// <param name="user">User to have claim added</param>
        /// <param name="claim">Claim to be added</param>
        /// <returns></returns>
        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            userClaimsTable.Insert(claim, user.Id);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns all claims for a given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            ClaimsIdentity identity = userClaimsTable.FindByUserId(user.Id);

            return Task.FromResult<IList<Claim>>(identity.Claims.ToList());
        }

        /// <summary>
        /// Removes a claim froma user
        /// </summary>
        /// <param name="user">User to have claim removed</param>
        /// <param name="claim">Claim to be removed</param>
        /// <returns></returns>
        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            userClaimsTable.Delete(user, claim);

            return Task.FromResult<object>(null);
        }
    }
}
