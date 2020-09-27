using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Prism.Regions;

namespace CalibrationInstructionsManager.Core.Regions
{
    public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
    {
        public StackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }
        // Anytime a new view is added to this region, the CollectionChangedEvent of this collection will be invoked
        // Then we are going to check the Action, if it's Add/Remove we are looping through all the new items/ old items
        // and then add/ remove the item to/ from the StackPanel.ChildrenCollection
        protected override void Adapt(IRegion region, StackPanel regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                // Adding incoming view to region
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement item in e.NewItems)
                    {
                        regionTarget.Children.Add(item);
                    }
                }

                // Removing a view from region
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (FrameworkElement item in e.OldItems)
                    {
                        regionTarget.Children.Remove(item);
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new Region();
        }
    }
}
