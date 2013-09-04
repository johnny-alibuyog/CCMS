using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Purchases;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Purchases
{
    public class TransactionsController : ControllerBase<TransactionsViewModel>
    {
        public TransactionsController(TransactionsViewModel viewModel) : base(viewModel)
        {
            this.MessageBus.Listen<CreditCardSelectedMessage>()
                .Subscribe(x => PopulateList(x.CreditCard));

            this.MessageBus.Listen<PurchaseAddedMessage>()
                .Subscribe(x => AddItem(x.Purchase));

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Delete = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.SelectedItem, x => x.Value != null));
            this.ViewModel.Delete.Subscribe(x => Delete());
        }

        public virtual void PopulateList(CreditCardInfoViewModel creditCard)
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<Purchase>()
                        .Where(x => x.CreditCard.Id == creditCard.Id)
                        .Select(x => new TransactionItemViewModel()
                        {
                            Id = x.Id,
                            Date = x.Date,
                            Details = x.Details,
                            CurrencyId = x.Amount.Currency.Id,
                            Amount = x.Amount.Value
                        })
                        .OrderByDescending(x => x.Date)
                        .ToList();

                    this.ViewModel.Items = new ObservableCollection<TransactionItemViewModel>(items);
                    transaction.Commit();
                }

            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Create()
        {
            try
            {
                var dialog = IoC.Container.Resolve<TransactionItemDialogService>();
                var item = dialog.ShowModal(this, "Create Transaction Item");
                if (item != null)
                {
                    this.ViewModel.Items.Insert(0, item);
                    this.ViewModel.SelectedItem = item;

                    this.MessageBox.ShowInformation(string.Format("{0} has been added.", item.Details));
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Delete()
        {
            try
            {
                var message = string.Format("Are you sure you want to delete details:{0}?", this.ViewModel.SelectedItem.Details);
                var result = this.MessageBox.ShowQuestion(message);
                if (result == MessageBoxResult.OK)
                    this.ViewModel.Items.Remove(this.ViewModel.SelectedItem);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void AddItem(Purchase item)
        {
            this.ViewModel.Items.Add(new TransactionItemViewModel()
            {
                Id = item.Id,
                Date = item.Date,
                Details = item.Details,
                CurrencyId = item.Amount.Currency.Id,
                Amount = item.Amount.Value,
            });
        }
    }
}
