using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CCMS.UI.Features.Purchases;

namespace CCMS.UI.Features.Purchases
{
    public class TransactionItemDialogService : DialogService<TransactionItemView, TransactionItemViewModel>
    {
        public TransactionItemDialogService(TransactionItemView view, TransactionItemViewModel viewModel)
            :base(view, viewModel) { }

        //#region IDialogService<TransactionItemViewModel> Members

        //private readonly TransactionItemView _view;

        //public TransactionItemViewModel ViewModel
        //{
        //    get
        //    {
        //        return _view.DataContext as TransactionItemViewModel;
        //    }
        //    set
        //    {
        //        value.Initialize();
        //        _view.DataContext = value;
        //    }
        //}

        //public TransactionItemViewModel Show()
        //{
        //    return Show(null, null);
        //}

        //public TransactionItemViewModel Show(object sender, params object[] args)
        //{
        //    var result = _view.ShowDialog();
        //    if (result != null && result == true)
        //        return this.ViewModel;
        //    else
        //        return null;
        //}

        //#endregion

        //#region Constructors

        //public TransactionItemDialogService(TransactionItemView view, TransactionItemViewModel viewModel)
        //{
        //    _view = view;
        //    _view.DataContext = viewModel;
        //    _view.Owner = App.CurrentWindow;
        //}

        //#endregion
    }
}
