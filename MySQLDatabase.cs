﻿using System.Data.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;

namespace AspNet.Identity.MySQL
{
     /// <summary>
     /// Class that encapsulates a MySQL database connections 
     /// and CRUD operations
     /// </summary>
    public class MySQLDatabase : IDisposable
    {
        private MySqlConnection _connection = null;

        private bool _newConnection = false;
        private String _connectionString;

        /// Default constructor which uses the "DefaultConnection" connectionString
        /// </summary>
        public MySQLDatabase()
            : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string name
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MySQLDatabase(string connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Constructor which takes the connection string name and lets us know to form a new MySqlConnection for every action
        /// Do not store a persistent connection object. Makes the class threadsafe.
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MySQLDatabase(string connectionStringName, bool newConnections)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _newConnection = newConnections;
        }

        /// <summary>
        /// Executes a non-query MySQL statement
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Optional parameters to pass to the query</param>
        /// <returns>The count of records affected by the MySQL statement</returns>
        public int Execute(string commandText, Dictionary<string, object> parameters)
        {
            int result = 0;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            MySqlConnection connection = null;
            try
            {
                connection = CurrentConnection;
                EnsureConnectionOpen(connection);
                using (var dbCmd = CreateCommand(connection, commandText, parameters))
                {
                    result = dbCmd.ExecuteNonQuery();
                }
            }
            finally
            {
                EnsureConnectionClosed(connection);
            }

            return result;
        }

        /// <summary>
        ///  way better than the ugly method that retuned a Dictionary of <String, Object> let caller specify how to pull out record from db
        /// </summary>
        /// <typeparam name="TRecord"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="recordIteration"></param>
        /// <returns></returns>
         public List<TRecord> ExecuteReader<TRecord>(String commandText, Dictionary<string, object> parameters, Func<DbDataReader, TRecord> recordIteration)
         {
          if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            MySqlConnection connection = null;
            try
            {
                connection = CurrentConnection;
                EnsureConnectionOpen(connection);
                using (var dbCmd = CreateCommand(connection, commandText, parameters))
                using (var dbReader = dbCmd.ExecuteReader())
                {
                    var records = new List<TRecord>();
                    while (dbReader.Read())
                    {
                        records.Add(recordIteration(dbReader));
                    }
                    return records;
                }
            }
            finally
            {
                EnsureConnectionClosed(connection);
            }
         }

        /// <summary>
        /// Executes a MySQL query that returns a single scalar value as the result.
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Optional parameters to pass to the query</param>
        /// <returns></returns>
        public object QueryValue(string commandText, Dictionary<string, object> parameters)
        {
            object result = null;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            MySqlConnection connection = null;
            try
            {
                connection = CurrentConnection;
                EnsureConnectionOpen(connection);
                using (var dbCmd = CreateCommand(connection, commandText, parameters))
                {
                    result = dbCmd.ExecuteScalar();
                }
            }
            finally
            {
                EnsureConnectionClosed(connection);
            }

            return result;
        }

        /// <summary>
        /// Executes a SQL query that returns a list of rows as the result.
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        /// <returns>A list of a Dictionary of Key, values pairs representing the 
        /// ColumnName and corresponding value</returns>
        public List<Dictionary<string, string>> Query(string commandText, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, string>> rows = null;
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            MySqlConnection connection = null;
            try
            {
                connection = CurrentConnection;
                EnsureConnectionOpen(connection);
                using (var dbCmd = CreateCommand(connection, commandText, parameters))
                using (var dbReader = dbCmd.ExecuteReader())
                {
                    rows = new List<Dictionary<string, string>>();
                    while (dbReader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (var i = 0; i < dbReader.FieldCount; i++)
                        {
                            var columnName = dbReader.GetName(i);
                            var columnValue = dbReader.IsDBNull(i) ? null : dbReader.GetString(i);
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                    return rows;
                }
            }
            finally
            {
                EnsureConnectionClosed(connection);
            }
        }

        /// <summary>
        /// Executes a sql insert which expects back an auto_increment'd field result
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public long Insert(string commandText, Dictionary<string, object> parameters)
        {
            long result = 0L;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            MySqlConnection connection = null;
            try
            {
                connection = CurrentConnection;
                EnsureConnectionOpen(connection);
                using (var dbCmd = CreateCommand(connection, commandText, parameters))
                {
                    dbCmd.ExecuteNonQuery();
                    result = dbCmd.LastInsertedId;
                }
            }
            finally
            {
                EnsureConnectionClosed(connection);
            }

            return result;
        }

        /// <summary>
        /// Opens a connection if not open
        /// </summary>
        private static void EnsureConnectionOpen(MySqlConnection connection)
        {
            if (connection == null)
            {
                return;
            }
            var retries = 3;
            if (connection.State == ConnectionState.Open)
            {
                return;
            }
            else
            {
                while (retries >= 0 && connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    retries--;
                    Thread.Sleep(30);
                }
            }
        }

        /// <summary>
        /// Closes a connection if open
        /// </summary>
        public void EnsureConnectionClosed(MySqlConnection connection)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            if (connection != null && _newConnection)
            {
                // we also want to dispose of connection at this point, since we create a new connection per action
                try
                {
                    connection.Dispose();
                }
                catch { }
            }
        }

         private MySqlConnection CurrentConnection
         {
             get { return _newConnection ? new MySqlConnection(_connectionString) : _connection; }
         }

        /// <summary>
        /// Creates a MySQLCommand with the given parameters
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        /// <returns></returns>
        private static MySqlCommand CreateCommand(MySqlConnection connection, string commandText, Dictionary<string, object> parameters)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            AddParameters(command, parameters);

            return command;
        }

        /// <summary>
        /// Adds the parameters to a MySQL command
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        private static void AddParameters(MySqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var param in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = param.Key;
                parameter.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Helper method to return query a string value 
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        /// <returns>The string value resulting from the query</returns>
        public string GetStrValue(string commandText, Dictionary<string, object> parameters)
        {
            Object value = QueryValue(commandText, parameters);
            if (value != null)
            {
                return Convert.ToString(QueryValue(commandText, parameters));
            }
            return null;
        }

        /// <summary>
        /// Helper method to return query a UInt32 value 
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        /// <returns>The UInt32 value resulting from the query</returns>
        public uint GetUInt32Value(string commandText, Dictionary<string, object> parameters)
        {
            Object value = QueryValue(commandText, parameters);
            if (value != null)
            {
                return Convert.ToUInt32(value);
            }
            return 0u;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
