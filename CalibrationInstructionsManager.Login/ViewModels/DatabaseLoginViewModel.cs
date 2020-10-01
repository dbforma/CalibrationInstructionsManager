using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Commands;
using CalibrationInstructionsManager.Core.Data;
using Prism.Commands;
using Prism.Regions;

namespace CalibrationInstructionsManager.Login.ViewModels
{
    public class DatabaseLoginViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Properties
        
        private string _host;
        private string _port;
        private string _username;
        private string _password;
        private string _database;

        // For User Input
        // public string Host { get { return _host; } set { SetProperty(ref _host, value); } }
        // public string Port { get { return _port; } set { SetProperty(ref _port, value); } }
        // public string Username { get { return _username; } set { SetProperty(ref _username, value); } }
        // public string Password { get { return _password; } set { SetProperty(ref _password, value); } }
        // public string Database { get { return _database; } set { SetProperty(ref _database, value); } }


        // IMC Kalidb
        public string Host { get; set; } = "10.0.2.167";
        public string Port { get; set; } = "5433";
        public string Username { get; set; } = "kalidb";
        public string Password { get; set; } = "imc";
        public string Database { get; set; } = "kalidb";

        // Localhost
        // public string Host { get; set; } = "localhost";
        // public string Port { get; set; } = "5432";
        // public string Username { get; set; } = "postgres";
        // public string Password { get; set; } = "admin";
        // public string Database { get; set; } = "kalidb";
      


        private IRegionManager _regionManager;
        private IPostgreSQLDatabase _repository { get; set; }

        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        public DelegateCommand PassLoginDataCommand { get; set; }

        public string this[string propertyName]
        {
            get
            {
                CollectErrors();
                return Errors.ContainsKey(propertyName) ? Errors[propertyName] : string.Empty;
            }
        }
        /// <summary>
        /// If Dictionary "Errors" has any Elements return true
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// If Dictionary has no errors set IsLoginDataComplete to true
        /// </summary>
        public bool IsLoginDataComplete => !HasErrors;

        /// <summary>
        /// IDataErrorInfo property that always retrieves string.Empty
        /// in order to implement the interface
        /// </summary>
        public string Error => string.Empty;

        #endregion // Properties

        public DatabaseLoginViewModel(IPostgreSQLDatabase repository, IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            _repository = repository;
            PassLoginDataCommand = new DelegateCommand(PassLoginDataToRepository, () => IsLoginDataComplete);
            // applicationCommands.EnableButtonCommand.RegisterCommand(PassLoginDataCommand);
        }

        public void PassLoginDataToRepository()
        {
            _repository.SetDatabaseLoginData(Host, Port, Username, Password, Database);
            if (_repository.IsConnectionEstablished())
            {
                _regionManager.RequestNavigate("NavigationRegion", "DefaultConfigurationsOverviewView");
            }
        }

        /// <summary>
        /// Collection of dictionary error messages
        /// </summary>
        private void CollectErrors()
        {
            Errors.Clear();
            if (string.IsNullOrEmpty(Host))
            {
                Errors.Add(nameof(Host), "Host is required.");
            }
            if (!(int.TryParse(Port, out int value)))
            {
                Errors.Add(nameof(Port), "Port is required and must be numeric.");
            }
            if (string.IsNullOrEmpty(Username))
            {
                Errors.Add(nameof(Username), "Username is required.");
            }
            if (string.IsNullOrEmpty(Password))
            {
                Errors.Add(nameof(Password), "Password is required.");
            }
            if (string.IsNullOrEmpty(Database))
            {
                Errors.Add(nameof(Database), "Database is required.");
            }

            PassLoginDataCommand.RaiseCanExecuteChanged();
        }
    }
}

