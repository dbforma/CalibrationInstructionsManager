namespace CalibrationInstructionsManager.Core.Data
{
    public class DefaultConfigurationsUpdateCommand
    {
        private IPostgreSQLDatabase _database;
        public DefaultConfigurationsUpdateCommand(IPostgreSQLDatabase database)
        {
            _database = database;
        }


    }
}
