using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common.Extentions;
using CCMS.UI.Features.Installments;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Installments
{
    public class InstallmentListController : ControllerBase<InstallmentListViewModel>
    {
        private ObservableCollection<KeyValuePair<string, string>> _currencies;
        private ObservableCollection<KeyValuePair<Guid, string>> _staffs;
        private ObservableCollection<KeyValuePair<Guid, string>> _transactionClassficications;

        public InstallmentListController(InstallmentListViewModel viewModel) : base(viewModel)
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
                var dialog = IoC.Container.Resolve<InstallmentDialogService>();
                dialog.ViewModel.TransactionClassifications = _transactionClassficications;
                dialog.ViewModel.Currencies = _currencies;
                dialog.ViewModel.Staffs = _staffs;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create Installment");
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Insert(InstallmentViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Installment?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;


                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<CreditCard>()
                        .Where(x => x.Id == App.Data.SelectedCreditCard.Id)
                        .Fetch(x => x.TransactionCurrency)
                        .FetchMany(x => x.Billings)
                        .ToFutureValue();

                    var creditCard = query.Value;

                    var installment = new Installment()
                    {
                        CreditCard = creditCard,
                        Date = item.Date,
                        Details = item.Details,
                        TransactionClassification = item.TransactionClassification.Key != Guid.Empty
                            ? session.Load<TransactionClassification>(item.TransactionClassification.Key) : null,
                        Staff = item.Staff.Key != Guid.Empty
                            ? session.Load<Staff>(item.Staff.Key) : null,
                        Terms = item.Terms,
                        InterestRate = item.InterestRate,
                        Amortization = creditCard.ResolveMoney(item.Amortization),
                        Interest = creditCard.ResolveMoney(item.Interest),
                        Amount = creditCard.ResolveMoney(item.Amount),
                        Balance = creditCard.ResolveMoney(item.Balance),
                    };

                    for (var i = 0; i < item.Terms; i++)
                    {
                        var date = installment.Date.AddMonths(i + 1);
                        var billing = creditCard.Billings
                            .Where(x =>
                                x.StartDate <= date &&
                                x.EndDate > date
                            )
                            .FirstOrDefault();

                        if (billing == null)
                        {
                            billing = new Core.Entities.Billing();
                            creditCard.AddBilling(billing);
                            billing.SetDuration(date);
                        }

                        billing.AddBillingItem(
                            new Core.Entities.InstallmentBillingItem(
                                installment, date
                            )
                        );
                    }

                    transaction.Commit();

                    item.Id = installment.Id;
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
                var message = string.Format("Are you sure you want to delete this installment: {0}?", item.Details);
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var creditCardQuery = session.Query<CreditCard>()
                        .Where(x => x.Id == App.Data.SelectedCreditCard.Id)
                        .Fetch(x => x.TransactionCurrency)
                        .FetchMany(x => x.Billings)
                        .ThenFetchMany(x => x.BillingItems)
                        .ToFutureValue();

                    var creditCard = creditCardQuery.Value;

                    var billings = creditCard.Billings
                        .Where(x => x.BillingItems
                            .OfType<Core.Entities.InstallmentBillingItem>()
                            .Any(o => o.Installment.Id == item.Id)
                        );

                    foreach (var billing in billings)
                    {
                        var itemToRemove = billing.BillingItems
                            .OfType<InstallmentBillingItem>()
                            .First(x => x.Installment.Id == item.Id);

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
                    var items = session.Query<Installment>()
                        .Where(x => x.CreditCard.Id == App.Data.SelectedCreditCard.Id)
                        .Select(x => new InstallmentViewModel()
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
                            Terms = x.Terms,
                            InterestRate = x.InterestRate,
                            Amortization = x.Amortization.Value,
                            Interest = x.Interest.Value,
                            Amount = x.Amount.Value,
                            Balance = x.Balance.Value,
                        })
                        .OrderByDescending(x => x.Date)
                        .ToList();

                    ViewModel.Items = new ObservableCollection<InstallmentViewModel>(items);
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
