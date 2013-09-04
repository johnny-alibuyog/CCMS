using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CCMS.UI.Features.Installments;

namespace CCMS.UI.Features.Installments
{
    public class InstallmentListDialogService : DialogService<InstallmentListView, InstallmentListViewModel>
    {
        public InstallmentListDialogService(InstallmentListView view, InstallmentListViewModel viewModel)
            : base(view, viewModel) { }

        //#region IDialogService<InstallmentListViewModel> Members 

        //private readonly InstallmentListView _view;

        //public InstallmentListViewModel ViewModel
        //{
        //    get { return _view.DataContext as InstallmentListViewModel; }
        //    set { _view.DataContext = value; }
        //}

        //public InstallmentListViewModel Show()
        //{
        //    return this.Show(null, null);
        //}

        //public InstallmentListViewModel Show(object sender, params object[] args)
        //{
        //    var result = _view.ShowDialog();
        //    if (result != null && result == true)
        //        return this.ViewModel;
        //    else
        //        return null;
        //}

        //#endregion

        //#region Constructors

        //public InstallmentListDialogService(InstallmentListView view, InstallmentListViewModel viewModel)
        //{
        //    _view = view;
        //    _view.DataContext = viewModel;
        //    _view.Owner = App.CurrentWindow;
        //}

        //#endregion
    }
}
