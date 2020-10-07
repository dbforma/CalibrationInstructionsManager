using System.Collections.Generic;
using System.Data;
using CalibrationInstructionsManager.Core.Models.Types;
using Npgsql;

namespace CalibrationInstructionsManager.Core.Data
{
    public class GetInstructionTypesCommand
    {
        private IPostgreSQLDatabase _database;
        public GetInstructionTypesCommand(IPostgreSQLDatabase database)
        {
            _database = database;
        }
    }
}
