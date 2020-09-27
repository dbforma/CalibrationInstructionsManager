using System;
using Prism.Services.Dialogs;

namespace CalibrationInstructionsManager.Core.Dialogs
{
    public static class IDialogServiceExtensions
    {
        public static void ShowMessageDialog(this IDialogService dialogService, string message, Action<IDialogResult> callBackAction)
        {
            var parameter = new DialogParameters();
            parameter.Add("myMessage", message);
            dialogService.ShowDialog("MessageDialogView", parameter, callBackAction);
        }
    }
}
