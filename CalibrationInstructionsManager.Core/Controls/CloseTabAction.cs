using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Prism.Regions;

namespace CalibrationInstructionsManager.Core.Controls
{
    public class CloseTabAction : TriggerAction<Button>
    {
        protected override void Invoke(object parameter)
        {
            var args = parameter as RoutedEventArgs;
            if (args == null)
                return;

            var tabItem = FindParent<TabItem>(args.OriginalSource as DependencyObject);
            if (tabItem == null)
                return;

            var tabControl = FindParent<TabControl>(tabItem);
            if (tabControl == null)
                return;

            // Grabing the region
            IRegion region = RegionManager.GetObservableRegion(tabControl).Value;
            if (region == null)
                return;
            // and removing the views from the region directly
            // Prism removes automatically the containing element (tabItem) from the hosting control (tabControl)
            // so we don't need to specifically say: tabControl.Items.Remove(tabItem.Content)

            // we need to check if we are allowed to remove a view from a region:
            RemoveItemFromRegion(tabItem.Content, region);

        }


        bool CanRemove(object item, NavigationContext navigationContext)
        {
            bool canRemove = true;

            // we check if the incoming item implements the IConfirmNavigationRequest interface
            var confirmRequestItem = item as IConfirmNavigationRequest;
            if (confirmRequestItem != null)
            {
                confirmRequestItem.ConfirmNavigationRequest(navigationContext, result =>
                {
                    canRemove = result;
                });
            }

            // we check if the item is a FrameworkElement
            var frameworkElement = item as FrameworkElement;
            if (frameworkElement != null && canRemove)
            {
                // we are checking if the Datacontext of the Frameworkelement implements IConfirmNavigationRequest Interface
                IConfirmNavigationRequest confirmRequestDataContext = frameworkElement.DataContext as IConfirmNavigationRequest;
                if (confirmRequestDataContext != null)
                {
                    confirmRequestDataContext.ConfirmNavigationRequest(navigationContext, result =>
                    {
                        canRemove = result;
                    });
                }
            }

            return canRemove;
        }

        void RemoveItemFromRegion(object item, IRegion region)
        {
            var navigationContext = new NavigationContext(region.NavigationService, null);
            if (CanRemove(item, navigationContext))
            {
                InvokeOnNavigatedFrom(item, navigationContext);
                region.Remove(item);
            }
        }

        void InvokeOnNavigatedFrom(object item, NavigationContext navigationContext)
        {
            var navigationAwareItem = item as INavigationAware;
            if (navigationAwareItem != null)
            {
                navigationAwareItem.OnNavigatedFrom(navigationContext);
            }

            var frameworkElement = item as FrameworkElement;
            if (frameworkElement != null)
            {
                INavigationAware navigationAwareDataContext = frameworkElement.DataContext as INavigationAware;
                if (navigationAwareDataContext != null)
                {
                    navigationAwareDataContext.OnNavigatedFrom(navigationContext);
                }
            }
        }

        static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            var parent = parentObject as T;

            if (parent != null)
                return parent;
            
            return FindParent<T>(parentObject);
        }
    }
}
