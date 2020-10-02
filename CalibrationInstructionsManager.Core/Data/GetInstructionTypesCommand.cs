using System.Collections.Generic;

namespace CalibrationInstructionsManager.Core.Data
{
    public class GetInstructionTypesCommand
    {
        private IPostgreSQLDatabase _database;
        public GetInstructionTypesCommand(IPostgreSQLDatabase database)
        {
            _database = database;
        }

        public LinkedList<string> GetMeasurementPointTypes;
    }
}
