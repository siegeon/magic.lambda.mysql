﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using MySql.Data.MySqlClient;
using magic.node;
using magic.signals.contracts;
using magic.lambda.mysql.utilities;

namespace magic.lambda.mysql
{
    /// <summary>
    /// [mysql.execute] slot for executing a non query SQL command.
    /// </summary>
    [Slot(Name = "mysql.execute")]
    public class Execute : ISlot
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            Executor.Execute(input, signaler.Peek<MySqlConnection>("mysql.connect"), (cmd) =>
            {
                input.Value = cmd.ExecuteNonQuery();
            });
        }
    }
}
