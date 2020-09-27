using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace CalibrationInstructionsManager.Core.Dialogs.ViewModels
{
    public class MessageDialogViewModel : BindableBase, IDialogAware
    {
        #region Properties

        private string _message;
        public string Message { get { return _message; } set { SetProperty(ref _message, value); } }

        public string Title { get; } = "Warning!";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand CloseDialogCommand { get; set; }

        #endregion // Properties

        public MessageDialogViewModel()
        {
            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        #region Methods

        private void CloseDialog()
        {
            var dialogResult = ButtonResult.OK;
            var parameter = new DialogParameters();
            parameter.Add("myParam", "User pressed OK Button.");

            RequestClose?.Invoke(new DialogResult(dialogResult, parameter));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("myMessage");
        }


        #endregion // Methods
    }
}
