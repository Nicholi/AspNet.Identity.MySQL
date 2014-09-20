using System.Collections.Generic;
using System.Data.Common;
using System.Security.Claims;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the UserClaims table in the MySQL Database
    /// </summary>
    public class UserClaimsTable
    {
        private MySQLDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserClaimsTable(MySQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(string userId)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            string commandText = "Select * from userclaims where UserId = @UserId";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UserId", userId } };

            var claims = _database.ExecuteReader(commandText, parameters, this.ReadClaim);
            claimsIdentity.AddClaims(claims);

            return claimsIdentity;
        }

        private Claim ReadClaim(DbDataReader dbReader)
        {
            return new Claim(dbReader.GetString("ClaimType"), dbReader.GetString("ClaimValue"));
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "Delete from userclaims where UserId = @UserId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public int Insert(Claim userClaim, string userId)
        {
            string commandText = "Insert into userclaims (ClaimValue, ClaimType, UserId) values (@Value, @Type, @UserId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Value", userClaim.Value);
            parameters.Add("@Type", userClaim.Type);
            parameters.Add("@UserId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, Claim claim)
        {
            string commandText = "Delete from userclaims where UserId = @UserId and ClaimValue = @Value and ClaimType = @Type";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.Id);
            parameters.Add("@Value", claim.Value);
            parameters.Add("@Type", claim.Type);

            return _database.Execute(commandText, parameters);
        }
    }
}
