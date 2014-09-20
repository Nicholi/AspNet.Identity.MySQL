using System;
using System.Collections.Generic;
using System.Data.Common;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the Users table in the MySQL Database
    /// </summary>
    public class UserTable<TUser>
        where TUser : IdentityUser
    {
        private MySQLDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTable(MySQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Ugly implementation for IQueryable on UserStore, we pull the whole db first than cast as Queryable
        /// </summary>
        /// <returns></returns>
        public List<TUser> GetUsers()
        {
            String commandText = "SELECT * FROM users";
            return _database.ExecuteReader(commandText, null, this.ReadUser);
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            string commandText = "Select Name from users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            string commandText = "Select Id from users where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(string userId)
        {
            TUser user = null;
            string commandText = "Select * from users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            var rows = _database.ExecuteReader(commandText, parameters, this.ReadUser);
            if (rows != null && rows.Count == 1)
            {
                user = rows[0];
            }

            return user;
        }

        private TUser ReadUser(DbDataReader dbReader)
        {
            var user = (TUser)Activator.CreateInstance(typeof(TUser));

            user.Id = dbReader.GetString("Id");
            user.Email = dbReader.GetString("Email");
            user.EmailConfirmed = dbReader.GetBoolean("EmailConfirmed");
            user.PasswordHash = dbReader.GetStringAsNullable("PasswordHash");
            user.SecurityStamp = dbReader.GetStringAsNullable("SecurityStamp");
            user.PhoneNumber = dbReader.GetStringAsNullable("PhoneNumber");
            user.PhoneNumberConfirmed = dbReader.GetBoolean("PhoneNumberConfirmed");
            user.TwoFactorEnabled = dbReader.GetBoolean("TwoFactorEnabled");
            user.LockoutEndDateUtc = dbReader.GetDateTimeAsNullable("LockoutEndDateUtc");
            user.LockoutEnabled = dbReader.GetBoolean("LockoutEnabled");
            user.AccessFailedCount = dbReader.GetInt32("AccessFailedCount");
            user.UserName = dbReader.GetString("UserName");

            return user;
        }

        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<TUser> GetUserByName(string userName)
        {
            string commandText = "Select * from users where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            return _database.ExecuteReader(commandText, parameters, this.ReadUser);
        }

        public List<TUser> GetUserByEmail(string email)
        {
            string commandText = "Select * from users where Email = @email";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@email", email } };

            return _database.ExecuteReader(commandText, parameters, this.ReadUser);
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            string commandText = "Select PasswordHash from users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);

            var passHash = _database.GetStrValue(commandText, parameters);
            if(string.IsNullOrEmpty(passHash))
            {
                return null;
            }

            return passHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update users set PasswordHash = @pwdHash where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@id", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };
            var result = _database.GetStrValue(commandText, parameters);

            return result;
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void Insert(TUser user)
        {
            string commandText = @"Insert into users (UserName, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled)
                values (@name, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", user.UserName);
            parameters.Add("@pwdHash", user.PasswordHash);
            parameters.Add("@SecStamp", user.SecurityStamp);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            var userId = _database.Insert(commandText, parameters);
            user.Id = userId.ToString();
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            string commandText = "Delete from users where Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            string commandText = @"Update users set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, 
                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled  
                WHERE Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", user.Id);
            parameters.Add("@userName", user.UserName);
            parameters.Add("@pswHash", user.PasswordHash);
            parameters.Add("@secStamp", user.SecurityStamp);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            return _database.Execute(commandText, parameters);
        }
    }
}
