using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CCMS.UI.Features.Installments;

namespace CCMS.UI.Features.Installments
{
    public class InstallmentDialogService : DialogService<InstallmentView, InstallmentViewModel>
    {
        public InstallmentDialogService(InstallmentView view, InstallmentViewModel viewModel)
            : base(view, viewModel) { }

        //#region IDialogService<InstallmentViewModel> Members

        //private readonly InstallmentView _view;

        //public InstallmentViewModel ViewModel
        //{
        //    get { return _view.DataContext as InstallmentViewModel; }
        //    set { _view.DataContext = value; }
        //}

        //public InstallmentViewModel Show()
        //{
        //    return this.Show(null, null);
        //}

        //public InstallmentViewModel Show(object sender, params object[] args)
        //{
        //    var result = _view.ShowDialog();
        //    if (result != null && result == true)
        //        return this.ViewModel;
        //    else
        //        return null;
        //}

        //#endregion

        //#region Constructors

        //public InstallmentDialogService(InstallmentView view, InstallmentViewModel viewModel)
        //{
        //    _view = view;
        //    _view.DataContext = viewModel;
        //    _view.Owner = App.CurrentWindow;
        //}

        //#endregion
    }
}
