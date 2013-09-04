using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CreditCards
{
    public class CreditCardListViewModel : ViewModelBase
    {
        private readonly CreditCardListController _controller;

        public virtual CreditCardViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<CreditCardViewModel> Items { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Providers { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public CreditCardListViewModel()
        {
            _controller = new CreditCardListController(this);

            // create commands
            //this.Create = new RelayCommand(_ =>
            //{
            //    var dialog = IoC.Container.Resolve<CreditCardDialogService>();
            //    dialog.ViewModel.Providers = this.Providers;
            //    dialog.ViewModel.Currencies = this.Currencies;

            //    var item = dialog.ShowModal(this, "Create Credit Card");
            //    if (item != null)
            //    {
            //        _service.Insert(item);

            //        this.Items.Insert(0, item);
            //        this.SelectedItem = item;
            //        this.AcceptChanges();
            //    }
            //});

            // update commands
            //this.Edit = new RelayCommand(param =>
            //{
            //    var item = this.Items.Single(x => x.Id == (Guid)param);
            //    var dialog = IoC.Container.Resolve<CreditCardDialogService>();
            //    dialog.ViewModel.HydrateWith(item);
            //    dialog.ViewModel.Providers = this.Providers;
            //    dialog.ViewModel.Currencies = this.Currencies;

            //    var value = dialog.ShowModal(this, "Edit Credit Card");
            //    if (value != null)
            //    {
            //        _service.Update(value);
            //        item.HydrateWith(value);

            //        this.SelectedItem = item;
            //        this.AcceptChanges();
            //    }
            //});

            // delete commands
            //this.Delete = new RelayCommand(param =>
            //{
            //    var item = this.Items.Single(x => x.Id == (Guid)param);
            //    var messageBox = IoC.Container.Resolve<IMessageBoxService>();
            //    var result = messageBox.ShowQuestion(string.Format(
            //        "Are you sure you want to delete account:{0}?",
            //        item.AccountName
            //    ));
            //    if (result == MessageBoxResult.OK)
            //    {
            //        _service.Delete(item);

            //        this.Items.Remove(item);
            //        this.SelectedItem = null;
            //        this.AcceptChanges();
            //    }
            //});
        }
    }
}
