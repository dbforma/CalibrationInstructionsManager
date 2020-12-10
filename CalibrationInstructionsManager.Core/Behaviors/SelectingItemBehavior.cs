// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using System.Windows;
// using System.Windows.Controls;
// using System.Windows.Data;
// using CalibrationInstructionsManager.Core.Models.Templates;
//
// namespace CalibrationInstructionsManager.Core.Behaviors
// {
//     public class SelectingItemAttachedProperty
//     {
//         public static readonly DependencyProperty SelectingItemProperty = DependencyProperty.RegisterAttached(
//             "SelectingItem",
//             typeof(ChannelSettingTemplate),
//             typeof(SelectingItemAttachedProperty),
//             new PropertyMetadata(default(ChannelSettingTemplate), OnSelectingItemChanged));
//
//         public static ChannelSettingTemplate GetSelectingItem(DependencyObject target)
//         {
//             return (ChannelSettingTemplate)target.GetValue(SelectingItemProperty);
//         }
//
//         public static void SetSelectingItem(DependencyObject target, ChannelSettingTemplate value)
//         {
//             target.SetValue(SelectingItemProperty, value);
//         }
//
//         public static ObservableCollection<ChannelSettingTemplate> itemView { get; set; }
//
//         static void OnSelectingItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
//         {
//             var grid = sender as DataGrid;
//             if (grid == null || grid.SelectedItem == null)
//                 return;
//
//             itemView = new ObservableCollection<ChannelSettingTemplate>();
//
//             foreach (var item in grid.Items)
//             {
//                 itemView.Add((ChannelSettingTemplate)item);
//             }
//            
//             // Works with .Net 4.5
//             grid.Dispatcher.InvokeAsync(() =>
//             {
//                 grid.UpdateLayout();
//                 grid.ScrollIntoView(grid.SelectedItem, null);
//             });
//
//             // Works with .Net 4.0
//             grid.Dispatcher.BeginInvoke((Action)(() =>
//             {
//                 grid.UpdateLayout();
//                 grid.ScrollIntoView(grid.SelectedItem, null);
//             }));
//         }
//     }
// }
