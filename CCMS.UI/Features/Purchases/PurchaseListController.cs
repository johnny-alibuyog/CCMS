using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common.Extentions;
using CCMS.UI.Features.Purchases;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Purchases
{
    public class PurchaseListController : ControllerBase<PurchaseListViewModel>
    {
        private ObservableCollection<KeyValuePair<string, string>> _currencies;
        private ObservableCollection<KeyValuePair<Guid, string>> _staffs;
        private ObservableCollection<KeyValuePair<Guid, string>> _transactionClassficications;

        public PurchaseListController(PurchaseListViewModel viewModel) : base(viewModel)
        {
            this.PopulateLookup();
            this.PopulateList();

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((Guid)x));
        }

        public virtual void Create()
        {
            try
            {
                var dialog = IoC.Container.Resolve<PurchaseDialogService>();
                dialog.ViewModel.TransactionClassifications = _transactionClassficications;
                dialog.ViewModel.Currencies = _currencies;
                dialog.ViewModel.Staffs = _staffs;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create Purchase");
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Insert(PurchaseViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Purchase?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Billing>()
                        .Where(x =>
                            x.CreditCard.Id == App.Data.SelectedCreditCard.Id &&
                            x.StartDate <= item.Date &&
                            x.EndDate > item.Date
                        )
                        .ToFutureValue();

                    var billing = query.Value;
                    if (billing == null)
                    {
                        billing = new Core.Entities.Billing();
                        billing.CreditCard = session.Get<Core.Entities.CreditCard>(App.Data.SelectedCreditCard.Id);
                        billing.SetDuration(item.Date);
                        session.Save(billing);
                    }

                    var billingItem = new Core.Entities.PurchaseBillingItem(new Purchase()
                    {
                        CreditCard = billing.CreditCard,
                        Date = item.Date,
                        Details = item.Details,
                        TransactionClassification = item.TransactionClassification.Key != Guid.Empty
                            ? session.Load<TransactionClassification>(item.TransactionClassification.Key) : null,
                        Staff = item.Staff.Key != Guid.Empty
                            ? session.Load<Staff>(item.Staff.Key) : null,
                        Amount = new Core.Entities.Money(
                            currency: session.Load<Core.Entities.Currency>(
                                App.Data.SelectedCreditCard.TransactionCurrencyId
                            ),
                            value: item.Amount)
                    });

                    billing.AddBillingItem(billingItem);

                    transaction.Commit();

                    item.Id = billingItem.Purchase.Id;
                }

                this.MessageBox.ShowInformation("Save has been successfully completed.");

                this.ViewModel.Items.Insert(0, item);
                this.ViewModel.SelectedItem = item;
                this.ViewModel.AcceptChanges();

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Delete(Guid id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                var message = string.Format("Are you sure you want to delete details: {0}?", item.Details);
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Core.Entities.Billing>()
                        .Where(x => x.BillingItems
                            .OfType<Core.Entities.PurchaseBillingItem>()
                            .Any(o => o.Purchase.Id == item.Id)
                        )
                        .Fetch(x => x.CreditCard)
                        .FetchMany(x => x.BillingItems)
                        .ToFutureValue();

                    var billing = query.Value;
                    if (billing != null)
                    {
                        var itemToRemove = billing.BillingItems
                            .OfType<Core.Entities.PurchaseBillingItem>()
                            .First(x => x.Purchase.Id == item.Id);

                        billing.RemoveBillingItem(itemToRemove);
                    }

                    transaction.Commit();
                }

                this.MessageBox.ShowInformation("Delete has been successfully completed.");

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;
                this.ViewModel.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void PopulateList()
        {
            try
            {
                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<Purchase>()
                        .Where(x => x.CreditCard.Id == App.Data.SelectedCreditCard.Id)
                        .Select(x => new PurchaseViewModel()
                        {
                            Id = x.Id,
                            Date = x.Date,
                            Details = x.Details,
                            TransactionClassification = x.TransactionClassification == null
                                ? new KeyValuePair<Guid, string>()
                                : new KeyValuePair<Guid, string>(
                                    x.TransactionClassification.Id,
                                    x.TransactionClassification.Name
                                ),
                            Staff = x.Staff == null
                                ? new KeyValuePair<Guid, string>()
                                : new KeyValuePair<Guid, string>(
                                    x.Staff.Id,
                                    x.Staff.Person.FirstName + " " +
                                    x.Staff.Person.MiddleName + " " +
                                    x.Staff.Person.LastName
                                ),
                            Amount = x.Amount.Value
                        })
                        .OrderByDescending(x => x.Date)
                        .ToList();

                    ViewModel.Items = new ObservableCollection<PurchaseViewModel>(items);
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void PopulateLookup()
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var staffs = session.Query<Staff>().Cacheable().ToList();
                    var classificatons = session.Query<TransactionClassification>().Cacheable().ToList();
                    var currencies = session.Query<Currency>().Cacheable().ToList();

                    _staffs = new ObservableCollection<KeyValuePair<Guid, string>>(staffs
                        .Select(x => new KeyValuePair<Guid, string>(x.Id, x.Person.Fullname))
                        .OrderBy(x => x.Value));

                    _transactionClassficications = new ObservableCollection<KeyValuePair<Guid, string>>(classificatons
                        .Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name))
                        .OrderBy(x => x.Value));

                    _currencies = new ObservableCollection<KeyValuePair<string, string>>(currencies
                        .Select(x => new KeyValuePair<string, string>(x.Id, x.Name))
                        .OrderBy(x => x.Value));

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }
    }
}
