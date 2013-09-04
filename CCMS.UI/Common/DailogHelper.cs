using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CCMS.UI.Common
{
    public static class DailogHelper
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult",
                typeof(bool?), typeof(DailogHelper), 
                new PropertyMetadata(DialogResultChanged)
            );

        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }
    }
}
