using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Exceptions;
using CCMS.Core.Entities;
using CCMS.Core.Services;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common.Extentions;
using CCMS.UI.Features.Payments;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Payments
{
    public class PaymentListController : ControllerBase<PaymentListViewModel>
    {
        private readonly IComputationService _calculator;

        private ObservableCollection<KeyValuePair<string, string>> _currencies;
        private ObservableCollection<KeyValuePair<Guid, string>> _staffs;
        private ObservableCollection<KeyValuePair<Guid, string>> _transactionClassficications;

        public PaymentListController(PaymentListViewModel viewModel)
            : base(viewModel)
        {
            _calculator = IoC.Container.Resolve<IComputationService>();

            this.PopulateLookup();
            this.PopulateList();

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((Guid)x));
        }

        private PaymentViewModel ComputeNewPayment()
        {
            var viewModel = IoC.Container.Resolve<PaymentViewModel>();

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Billing>()
                    .Where(x =>
                        //x.StartDate < DateTime.Today &&
                        x.EndDate < DateTime.Today &&
                        x.BillingStatus == BillingStatus.Unpaid &&
                        x.CreditCard.Id == App.Data.SelectedCreditCard.Id
                    )
                    .OrderByDescending(x => x.DueDate)
                    .ToFutureValue();

                var billing = query.Value;
                if (billing != null)
                {
                    viewModel.PaymentDueDate = billing.DueDate;
                    viewModel.TotalAmountDue = billing.SettlementBalance.Value;
                    viewModel.TotalMinimumAmountDue = _calculator.Compute<MinimumPaymentSetting>(billing.SettlementBalance).Value;
                    //viewModel.Date = DateTime.Today;
                    viewModel.Amount = viewModel.TotalAmountDue;
                }

                transaction.Commit();
            }

            return viewModel;
        }

        public virtual void Create()
        {
            try
            {
                var dialog = IoC.Container.Resolve<PaymentDialogService>();
                dialog.ViewModel = this.ComputeNewPayment();
                dialog.ViewModel.TransactionClassifications = _transactionClassficications;
                dialog.ViewModel.Currencies = _currencies;
                dialog.ViewModel.Staffs = _staffs;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create Payment");
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Insert(PaymentViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Payment?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;


                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    //var query = session.Query<CreditCard>()
                    //    .Where(x => x.Id == App.Data.SelectedCreditCard.Id)
                    //    .FetchMany(x => x.Billings)
                    //    .ToFutureValue();

                    //var creditCard = query.Value;
                    //var payment = new Core.Entities.Payment();
                    //creditCard.AddPayment(payment);

                    //var billings = creditCard.Billings
                    //    .Where(x => x.BillingStatus == BillingStatus.Unpaid)
                    //    .OrderBy(x => x.DueDate)
                    //    .ToList();

                    //payment.Date = item.Date;
                    //payment.TransactionClassification = item.TransactionClassification.Key != Guid.Empty
                    //    ? session.Load<TransactionClassification>(item.TransactionClassification.Key)
                    //    : null;
                    //payment.Staff = item.Staff.Key != Guid.Empty
                    //    ? session.Load<Staff>(item.Staff.Key)
                    //    : null;
                    //payment.Amount = creditCard.ResolveMoney(item.Amount);

                    //foreach (var billing in billings)
                    //{
                    //    payment.AddPaymentItem(new PaymentItem(billing));

                    //    var paymentCredit = payment.GetPaymentCredit();
                    //    if (paymentCredit != null && paymentCredit.Value <= 0)
                    //        break;
                    //}

                    var userAlias = (User)null;
                    var billingAlias = (Billing)null;
                    var currencyAlias = (Currency)null;
                    var billingItemAlias = (BillingItem)null;
                    var creditCardAlias = (CreditCard)null;
                    var providerAlias = (CreditCardProvider)null;

                    var billingQuery = session.QueryOver<Billing>(() => billingAlias)
                       .Left.JoinAlias(() => billingAlias.CreditCard, () => creditCardAlias)
                       .Left.JoinAlias(() => billingAlias.BillingItems, () => billingItemAlias)
                       .Left.JoinAlias(() => creditCardAlias.User, () => userAlias)
                       .Left.JoinAlias(() => creditCardAlias.Provider, () => providerAlias)
                       .Left.JoinAlias(() => creditCardAlias.TransactionCurrency, () => currencyAlias)
                       .Where(() =>
                           userAlias.Id == App.Data.CurrentUser.Id &&
                           creditCardAlias.Id == App.Data.SelectedCreditCard.Id &&
                           billingAlias.EndDate < DateTime.Today &&
                           billingAlias.BillingStatus == BillingStatus.Unpaid
                       )
                       .OrderByAlias(() => billingAlias.EndDate).Desc
                       .TransformUsing(Transformers.DistinctRootEntity)
                       .Future();

                    if (billingQuery.FirstOrDefault() == null)
                    {
                        billingQuery = session.QueryOver<Billing>(() => billingAlias)
                            .Left.JoinAlias(() => billingAlias.CreditCard, () => creditCardAlias)
                            .Left.JoinAlias(() => billingAlias.BillingItems, () => billingItemAlias)
                            .Left.JoinAlias(() => creditCardAlias.User, () => userAlias)
                            .Left.JoinAlias(() => creditCardAlias.Provider, () => providerAlias)
                            .Left.JoinAlias(() => creditCardAlias.TransactionCurrency, () => currencyAlias)
                            .Where(() =>
                                userAlias.Id == App.Data.CurrentUser.Id &&
                                creditCardAlias.Id == App.Data.SelectedCreditCard.Id &&
                                billingAlias.BillingStatus == BillingStatus.Unpaid
                            )
                            .OrderByAlias(() => billingAlias.StartDate).Asc
                            .TransformUsing(Transformers.DistinctRootEntity)
                            .Future();
                    }

                    var currentBilling = billingQuery.FirstOrDefault();
                    if (currentBilling == null)
                        throw new BusinessException("There is no billing yet to pay for.");

                    // merge previous to current billing
                    var previousBillings = billingQuery.Where(x => x != currentBilling);
                    currentBilling.MergeWith(previousBillings);

                    var payment = new Payment()
                    {
                        Date = item.Date,
                        TransactionClassification = item.TransactionClassification.Key != Guid.Empty
                            ? session.Load<TransactionClassification>(item.TransactionClassification.Key)
                            : null,
                        Staff = item.Staff.Key != Guid.Empty
                            ? session.Load<Staff>(item.Staff.Key)
                            : null,
                        Amount = new Money(
                            currency: session.Load<Currency>(
                                App.Data.SelectedCreditCard.TransactionCurrencyId
                            ),
                            value: item.Amount
                        ),
                    };

                    currentBilling.AddPayment(payment);

                    transaction.Commit();

                    item.Id = payment.Id;
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
                var message = string.Format("Are you sure you want to delete payment dated: {0}?", item.Date.ToString("yyyy-MM-dd"));
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Billing>()
                        .Where(x => x.Payments.Any(o => o.Id == item.Id))
                        .Fetch(x => x.CreditCard)
                        .FetchMany(x => x.Payments)
                        .ToFutureValue();

                    var billing = query.Value;
                    var payment = billing.Payments.FirstOrDefault(x => x.Id == item.Id);
                    billing.RemovePayment(payment);

                    transaction.Commit();

                    //var query = session.Query<Core.Entities.Payment>()
                    //    .Where(x => x.Id == item.Id)
                    //    .Fetch(x => x.Billing)
                    //    .FetchMany(x => x.PaymentItems)
                    //    .ThenFetch(x => x.Billing)
                    //    .ToFutureValue();

                    //var payment = query.Value;

                    //foreach (var paymentItem in payment.PaymentItems.ToArray())
                    //    payment.RemovePaymentItem(paymentItem);

                    //session.Delete(payment);
                    //transaction.Commit();
                }

                this.MessageBox.ShowInformation("Save has been successfully completed.");

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
                    var items = session.Query<Payment>()
                        .Where(x => x.Billing.CreditCard.Id == App.Data.SelectedCreditCard.Id)
                        .Select(x => new PaymentViewModel()
                        {
                            Id = x.Id,
                            Date = x.Date,
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

                    ViewModel.Items = new ObservableCollection<PaymentViewModel>(items);
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
