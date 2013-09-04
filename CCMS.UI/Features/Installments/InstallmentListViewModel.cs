using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.Billings;
using CCMS.UI.Features.Installments;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Installments
{
    public class InstallmentListViewModel : ViewModelBase
    {
        private readonly InstallmentListController _controller;

        public virtual InstallmentViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<InstallmentViewModel> Items { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        #region Constructors

        public InstallmentListViewModel()
        {
            _controller = new InstallmentListController(this);

            ////// Create commands
            ////this.Create = new RelayCommand(_ =>
            ////{
            ////    var dialog = IoC.Container.Resolve<InstallmentDialogService>();
            ////    var item = dialog.ShowModal(this, "Create Installment");
            ////    if (item != null)
            ////    {
            ////        _service.Insert(item);

            ////        this.Items.Insert(0, item);
            ////        this.SelectedItem = item;
            ////        this.AcceptChanges();
            ////    }
            ////});

            //// Delete commands
            //this.Delete = new RelayCommand(param =>
            //{
            //    var item = this.Items.Single(x => x.Id == (Guid)param);
            //    var messageBox = IoC.Container.Resolve<IMessageBoxService>();
            //    var message = string.Format("Are you sure you want to delete this installment: {0}?", item.Details);
            //    var result = messageBox.ShowQuestion(message);
            //    if (result == MessageBoxResult.OK)
            //    {
            //        _service.Delete(item);

            //        this.Items.Remove(item);
            //        this.SelectedItem = null;
            //        this.AcceptChanges();
            //    }
            //});
        }

        #endregion
    }
}
