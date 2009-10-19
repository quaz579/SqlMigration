﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlMigration
{
    public class RunSqlFileTask : MigrationTask
    {
        private readonly IFileIO _fileIo;
        private readonly ISqlRunner _sqlRunner;

        public RunSqlFileTask(Arguments arguments, IFileIO fileIo, ISqlRunner sqlRunner)
            : base(arguments)
        {
            _fileIo = fileIo;
            _sqlRunner = sqlRunner;
        }

        public override int RunTask()
        {
            //grab the filepath
            string sqlFilePath = base.Arguments.GetArgumentValue(TaskTypeConstants.RunSqlFileTask);

            //create the migration to pass into the sql runner
            var migration = new TSqlMigration(sqlFilePath, _fileIo);

            //run inside transaction?
            bool runInsideTransaction = !Arguments.DoesArgumentExist(ArgumentConstants.RunWithoutTransactionArg);

            //run it
            _sqlRunner.ConnectionString = base.Arguments.GetArgumentValue(ArgumentConstants.ConnectionStringArg);
            return _sqlRunner.StartMigrations(new List<Migration> { migration }, runInsideTransaction, false);
        }
    }
}