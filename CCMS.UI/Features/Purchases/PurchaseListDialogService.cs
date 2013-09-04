using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CCMS.UI.Features.Purchases;

namespace CCMS.UI.Features.Purchases
{
    public class PurchaseListDialogService : DialogService<PurchaseListView, PurchaseListViewModel>
    {
        public PurchaseListDialogService(PurchaseListView view, PurchaseListViewModel viewModel)
            : base(view, viewModel) { }

        //#region IDialogService<PurchaseListViewModel> Members

        //private readonly PurchaseListView _view;

        //public PurchaseListViewModel ViewModel
        //{
        //    get { return _view.DataContext as PurchaseListViewModel; }
        //    set { _view.DataContext = value; }
        //}

        //public PurchaseListViewModel Show()
        //{
        //    return this.Show(null, null);
        //}

        //public PurchaseListViewModel Show(object sender, params object[] args)
        //{
        //    var result = _view.ShowDialog();
        //    if (result != null && result == true)
        //        return this.ViewModel;
        //    else
        //        return null;
        //}

        //#endregion

        //#region Constructors

        //public PurchaseListDialogService(PurchaseListView view, PurchaseListViewModel viewModel)
        //{
        //    _view = view;
        //    _view.DataContext = viewModel;
        //    _view.Owner = App.CurrentWindow;
        //}

        //#endregion
    }
}
