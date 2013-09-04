using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CCMS.UI.Features.Purchases;

namespace CCMS.UI.Features.Purchases
{
    public class PurchaseDialogService : DialogService<PurchaseView, PurchaseViewModel>
    {
        public PurchaseDialogService(PurchaseView view, PurchaseViewModel viewModel)
            : base(view, viewModel) { }

        //#region IDialogService<PurchaseViewModel> Members

        //private readonly PurchaseView _view;

        //public PurchaseViewModel ViewModel
        //{
        //    get { return _view.DataContext as PurchaseViewModel;  }
        //    set { _view.DataContext = value; }
        //}

        //public PurchaseViewModel Show()
        //{
        //    return Show(null, null);
        //}

        //public PurchaseViewModel Show(object sender, params object[] args)
        //{
        //    var result = _view.ShowDialog();
        //    if (result != null && result == true)
        //        return this.ViewModel;
        //    else
        //        return null;
        //}

        //#endregion

        //#region Constructors

        //public PurchaseDialogService(PurchaseView view, PurchaseViewModel viewModel)
        //{
        //    _view = view;
        //    _view.DataContext = viewModel;
        //    _view.Owner = App.CurrentWindow;
        //}

        //#endregion
    }
}
