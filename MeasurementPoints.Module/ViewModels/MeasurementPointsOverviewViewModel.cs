using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Business;
using CalibrationInstructionsManager.Core.Models;
using Prism.Commands;
using Prism.Regions;

namespace MeasurementPoints.Module.ViewModels
{
    public class MeasurementPointsOverviewViewModel : ViewModelBase
    {
        #region Properties

        private IRegionManager _regionManager;
        private readonly IRepository _repository; // Verwendungsabhängigkeit aufgelöst

        private ObservableCollection<MeasurementPoint> _measurementPoints;

        public ObservableCollection<MeasurementPoint> MeasurementPoints
        {
            get { return _measurementPoints; }
            set { SetProperty(ref _measurementPoints, value); }
        }

        public List<string> PropertiesList { get; set; }

        public DelegateCommand<MeasurementPoint> MeasurementPointSelectedCommand { get; private set; }

        //public ObservableCollection<MeasurementPoint> MeasurementPointsCollection { get; set; }

        #endregion // Properties

        public MeasurementPointsOverviewViewModel(IRegionManager regionManager, IRepository repository)
        {
            MeasurementPointSelectedCommand = new DelegateCommand<MeasurementPoint>(MeasurementPointSelected);
            _regionManager = regionManager;
            _repository = repository; // Erzeugungsabhängigkeit aufgelöst
            MeasurementPoints =
                new ObservableCollection<MeasurementPoint>(_repository
                    .GetMeasurementPoints(5)); // Verwendungsabhängigkeit aufgelöst
            PropertiesList = new List<string>(_repository.GetPropertiesList());
        }

        #region Methods

        private void MeasurementPointSelected(MeasurementPoint measurementPoint)
        {
            if (measurementPoint == null)
                return;
            var parameters = new NavigationParameters();
            parameters.Add("measurementPoint", measurementPoint);

            if (measurementPoint != null)
            {
                _regionManager.RequestNavigate("MeasurementPointsDetailRegion", "MeasurementPointsDetailView",
                    parameters);
            }
        }


        /// <summary>
        /// Search bar implementation for Channel Settings
        /// TODO: Adjust copy&paste code from old project
        /// </summary>
        // public virtual ObservableCollection<IChannelSettingTemplates> ChannelSettingsListContainsSearchKeyword { get; set; }
        // private string _searchKeyword;
        //
        // public string SearchKeyword
        // {
        //     get
        //     {
        //         return _searchKeyword;
        //     }
        //
        //     set
        //     {
        //         if (_searchKeyword == value)
        //         {
        //             return;
        //         }
        //         _searchKeyword = value;
        //
        //         if (value != null)
        //         {
        //             ChannelSettingsListContainsSearchKeyword = new ObservableCollection<IChannelSettingTemplates>();
        //
        //             var QueryChannelSettingsWithKeyword =
        //                 from match in Templates.ChannelSettingTemplates
        //                 where match.Name.ToLower().Contains(_searchKeyword.ToLower())
        //                 select match;
        //
        //             ChannelSettingsListContainsSearchKeyword.AddRange(QueryChannelSettingsWithKeyword);
        //         }
        //
        //         else
        //         {
        //             ChannelSettingsListContainsSearchKeyword = new ObservableCollection<IChannelSettingTemplates>(Templates.ChannelSettingsTemplates.ToList());
        //         }
        //     }
        // }
        #endregion // Methods
    }
}
