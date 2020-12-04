using System;
using CalibrationInstructionsManager.Core.Commands;
using CalibrationInstructionsManager.Core.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace CalibrationInstructionsManager.ViewModels
{
    public class CalibrationInstructionsManagerShellViewModel : BindableBase
    {
        #region Properties

        private string _title = "Calibration Instructions Manager";
        public string Title { get { return _title; } set { SetProperty(ref _title, value); } }

        private readonly IRegionManager _regionManager;

        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands { get { return _applicationCommands; } set { SetProperty(ref _applicationCommands, value); } }

        public DelegateCommand<string> NavigateCommand { get; set; }

        public DelegateCommand<IPostgresql> WriteToDatabaseCommand { get; set; }

        #endregion // Properties

        public CalibrationInstructionsManagerShellViewModel(IApplicationCommands applicationCommands, IRegionManager regionManager)
        {
            ApplicationCommands = applicationCommands;
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            applicationCommands.EnableButtonCommand.RegisterCommand(NavigateCommand);

            WriteToDatabaseCommand = new DelegateCommand<IPostgresql>(WriteToDatabase);
            applicationCommands.WriteToDatabaseCommand.RegisterCommand(WriteToDatabaseCommand);
        }

        private void WriteToDatabase(IPostgresql database)
        {
            Console.WriteLine("Writing to database");
        }

        public CalibrationInstructionsManagerShellViewModel()
        {
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("NavigationRegion", uri);
        }
    }
}
