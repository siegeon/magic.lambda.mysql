﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using MySql.Data.MySqlClient;

namespace magic.lambda.mysql.helpers
{
    /*
     * Internal helper class to create a MySqlConnection lazy, such that it is not actuall created
     * before it's actually de-referenced.
     */
    internal sealed class MySqlConnectionWrapper : IDisposable
    {
        readonly Lazy<MySqlConnection> _connection;

        public MySqlConnectionWrapper(string connectionString)
        {
            _connection = new Lazy<MySqlConnection>(() =>
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();

                /*
                 * This looks a bit dirty, but to make sure we always treat dates
                 * as UTC, and that they're always stored as UTC, this is necessary.
                 */
                using (var cmd = new MySqlCommand("set time_zone = '+00:00'"))
                {
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                }
                return connection;
            });
        }

        /*
         * Property to retrieve underlying MySQL connection.
         */
        public MySqlConnection Connection => _connection.Value;

        public void Dispose()
        {
            if (_connection.IsValueCreated)
                _connection.Value.Dispose();
        }
    }
}
