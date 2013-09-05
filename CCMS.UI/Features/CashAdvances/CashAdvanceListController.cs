using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.Core.Services;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common.Extentions;
using CCMS.UI.Features.CashAdvances;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CashAdvances
{
    public class CashAdvanceListController : ControllerBase<CashAdvanceListViewModel>
    {
        private ObservableCollection<KeyValuePair<string, string>> _currencies;
        private ObservableCollection<KeyValuePair<Guid, string>> _staffs;
        private ObservableCollection<KeyValuePair<Guid, string>> _transactionClassficications;

        public CashAdvanceListController(CashAdvanceListViewModel viewModel) : base(viewModel)
        {

            this.PopulateLookup();
            this.PopulateList();
            
            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((Guid)x));
        }

        private void ComputeFee(CashAdvanceViewModel viewModel)
        {
            var _calculator = IoC.Container.Resolve<IComputationService>();
            var serviceFee = _calculator.Compute<ServiceFeeSetting>(viewModel.Amount);
            viewModel.ServiceFee = serviceFee != null ? serviceFee.Value : 0M;
        }

        public virtual void Create()
        {
            try
            {
                var dialog = IoC.Container.Resolve<CashAdvanceDialogService>();
                dialog.ViewModel.TransactionClassifications = _transactionClassficications;
                dialog.ViewModel.Currencies = _currencies;
                dialog.ViewModel.Staffs = _staffs;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
                dialog.ViewModel.WhenAny(x => x.Amount, x => x.Value).Subscribe(x => ComputeFee(dialog.ViewModel));
                dialog.ShowModal(this, "Create Cash Advance");
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Insert(CashAdvanceViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Cash Advance?");
                var result = this.MessageBox.Confirm(message);
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
                        .Fetch(x => x.CreditCard)
                        .ThenFetch(x => x.Provider)
                        .ThenFetchMany(x => x.ComputationSettings)
                        .ToFutureValue();

                    var billing = query.Value;
                    if (billing == null)
                    {
                        billing = new Billing();
                        billing.CreditCard = session.Get<CreditCard>(App.Data.SelectedCreditCard.Id);
                        billing.SetDuration(item.Date);
                        session.Save(billing);
                    }

                    // cash advance billing item
                    var billingItem = new CashAdvanceBillingItem(
                        new CashAdvance()
                        {
                            CreditCard = billing.CreditCard,
                            Date = item.Date,
                            Details = item.Details,
                            TransactionClassification = item.TransactionClassification.Key != Guid.Empty
                                ? session.Load<TransactionClassification>(item.TransactionClassification.Key) : null,
                            Staff = item.Staff.Key != Guid.Empty
                                ? session.Load<Staff>(item.Staff.Key) : null,
                            Amount = billing.CreditCard.ResolveMoney(item.Amount)
                        }
                    );

                    billing.AddBillingItem(billingItem);

                    // service fee
                    var serviceFee = billing.CreditCard.Compute<ServiceFeeSetting>(billingItem.Amount);
                    if (serviceFee != null)
                    {
                        billing.AddBillingItem(new FeeBillingItem()
                        {
                            Date = item.Date,
                            Details = string.Format("Service fee for cash advance {0}", billingItem.Amount.ToString()),
                            Amount = serviceFee
                        });
                    }

                    transaction.Commit();

                    item.Id = billingItem.CashAdvance.Id;
                }

                this.ViewModel.Items.Insert(0, item);
                this.ViewModel.SelectedItem = item;
                this.ViewModel.AcceptChanges();

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Delete(Guid id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                var message = string.Format("Are you sure you want to delete details: {0}?", item.Details);
                var result = this.MessageBox.Confirm(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Core.Entities.Billing>()
                        .Where(x => x.BillingItems
                            .OfType<Core.Entities.CashAdvanceBillingItem>()
                            .Any(o => o.CashAdvance.Id == item.Id)
                        )
                        .Fetch(x => x.CreditCard)
                        .FetchMany(x => x.BillingItems)
                        .ToFutureValue();

                    var billing = query.Value;
                    if (billing != null)
                    {
                        var itemToRemove = billing.BillingItems
                            .OfType<Core.Entities.CashAdvanceBillingItem>()
                            .First(x => x.CashAdvance.Id == item.Id);

                        billing.RemoveBillingItem(itemToRemove);
                    }

                    transaction.Commit();
                }

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;
                this.ViewModel.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void PopulateList()
        {
            try
            {
                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<Core.Entities.CashAdvance>()
                        .Where(x => x.CreditCard.Id == App.Data.SelectedCreditCard.Id)
                        .Select(x => new CashAdvanceViewModel()
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

                    ViewModel.Items = new ObservableCollection<CashAdvanceViewModel>(items);
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
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
                this.MessageBox.Warn(ex.Message, ex);
            }
        }
    }
}
