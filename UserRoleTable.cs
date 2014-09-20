using System;
using System.Collections.Generic;
using System.Data.Common;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the UserRoles table in the MySQL Database
    /// </summary>
    public class UserRolesTable
    {
        private MySQLDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserRolesTable(MySQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(string userId)
        {
            string commandText = "Select r.Name from userroles AS ur, roles AS r where ur.UserId = @userId and ur.RoleId = r.Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return _database.ExecuteReader(commandText, parameters, this.ReadRoleName);
        }

        private String ReadRoleName(DbDataReader dbReader)
        {
            return dbReader.GetString("Name");
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "Delete from userroles where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, string roleId)
        {
            string commandText = "Insert into userroles (UserId, RoleId) values (@userId, @roleId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", user.Id);
            parameters.Add("@roleId", roleId);

            return _database.Execute(commandText, parameters);
        }
    }
}
