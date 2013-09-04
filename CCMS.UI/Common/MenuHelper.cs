using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CCMS.UI.Common
{
    public static class MenuHelper
    {
        public static readonly DependencyProperty MenuPlacementProperty = 
            DependencyProperty.RegisterAttached("MenuPlacement",
                typeof(PlacementMode), typeof(MenuHelper),
                new FrameworkPropertyMetadata(PlacementMode.Bottom, 
                    FrameworkPropertyMetadataOptions.Inherits, 
                    new PropertyChangedCallback(OnMenuPlacementChanged)
                )
            );

        public static PlacementMode GetMenuPlacement(DependencyObject obj)
        {
            return (PlacementMode)obj.GetValue(MenuPlacementProperty);
        }

        public static void SetMenuPlacement(DependencyObject obj, PlacementMode value)
        {
            obj.SetValue(MenuPlacementProperty, value);
        }

        private static void OnMenuPlacementChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var menuItem = obj as MenuItem;
            if (menuItem == null)
                return;

            if (menuItem.IsLoaded)
                SetPopupPlacement(menuItem, (PlacementMode)e.NewValue);
            else
                menuItem.Loaded += new RoutedEventHandler((m, v) => SetPopupPlacement(menuItem, (PlacementMode)e.NewValue));
        }

        private static void SetPopupPlacement(MenuItem menuItem, PlacementMode placementMode)
        {
            var popup = menuItem.Template.FindName("PART_Popup", menuItem) as Popup;
            if (popup == null)
                return;

            popup.Placement = placementMode;
        }
    }
}
