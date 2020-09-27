//TODO: Adjust with properties used in ViewModels

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;
using Prism.Regions;

namespace CalibrationInstructionsManager.Core
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        private string _title;
        public string Title { get { return _title; } set { SetProperty(ref _title, value); } }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}