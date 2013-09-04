using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CCMS.UI.Features;

namespace CCMS.UI.Features
{
    public interface IDialogService<TView, TViewModel>
        where TView : Window
        where TViewModel : ViewModelBase
    {
        TView View { get; }
        TViewModel ViewModel { get; set; }

        // non modal members
        void Show();
        void Show(object sender, params object[] args);
        void Close();

        // modal members
        TViewModel ShowModal();
        TViewModel ShowModal(object sender, params object[] args);
    }
}
