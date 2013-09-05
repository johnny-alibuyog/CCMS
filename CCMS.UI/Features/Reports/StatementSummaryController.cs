using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Extentions;
using CCMS.Core.Entities;
using CCMS.UI.Features.Reports;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;

namespace CCMS.UI.Features.Reports
{
    public class StatementSummaryController : ControllerBase<StatementSummaryViewModel>
    {
        public StatementSummaryController(StatementSummaryViewModel viewModel)
            : base(viewModel)
        {
            this.Populate();
        }

        public void Populate()
        {
            try
            {
                var billings = (IEnumerable<Billing>)null;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var userAlias = (User)null;
                    var billingAlias = (Billing)null;
                    var currencyAlias = (Currency)null;
                    var billingItemAlias = (BillingItem)null;
                    var creditCardAlias = (CreditCard)null;
                    var providerAlias = (CreditCardProvider)null;

                    billings = session.QueryOver<Billing>(() => billingAlias)
                        .Left.JoinAlias(() => billingAlias.CreditCard, () => creditCardAlias)
                        .Left.JoinAlias(() => billingAlias.BillingItems, () => billingItemAlias)
                        .Left.JoinAlias(() => creditCardAlias.User, () => userAlias)
                        .Left.JoinAlias(() => creditCardAlias.Provider, () => providerAlias)
                        .Left.JoinAlias(() => creditCardAlias.TransactionCurrency, () => currencyAlias)
                        .Where(() =>
                            userAlias.Id == App.Data.CurrentUser.Id &&
                            billingAlias.EndDate < DateTime.Today &&
                            billingAlias.BillingStatus == BillingStatus.Unpaid
                        )
                        .OrderByAlias(() => billingAlias.EndDate).Desc
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .List();

                    var billingGroups = billings
                        .GroupBy(x => new
                        {
                            x.CreditCard,
                            x.CreditCard.TransactionCurrency
                        });

                    ViewModel.Items = new ObservableCollection<StatementSummaryItemViewModel>();

                    foreach (var group in billingGroups)
                    {
                        var item = new StatementSummaryItemViewModel();
                        var currentBilling = group.FirstOrDefault();
                        var previousBillings = group.Where(x => x != currentBilling);
                        currentBilling.MergeWith(previousBillings);

                        item.AccountName = group.Key.CreditCard.AccountName;
                        item.AccountNumber = group.Key.CreditCard.AccountNumber;
                        item.Provider = group.Key.CreditCard.Provider.Name;
                        item.Currency = group.Key.TransactionCurrency.Name;

                        item.PreviousBalance = currentBilling.PreviousBillingAmount.GetValue();
                        item.PaymentsCredits = currentBilling.GetPaymentsAndCredits().GetValue();

                        if (currentBilling != null)
                        {
                            item.PurchasesInstallments = currentBilling.BillingItems
                                .Where(x =>
                                    x is PurchaseBillingItem ||
                                    x is InstallmentBillingItem
                                )
                                .Sum(x => x.Amount.GetValue());

                            item.CashAdvances = currentBilling.BillingItems
                                .Where(x => x is CashAdvanceBillingItem)
                                .Sum(x => x.Amount.GetValue());

                            item.InterestsFeesCharges = currentBilling.BillingItems
                                .Where(x =>
                                    x is InterestBillingItem ||
                                    x is ChargeBillingItem ||
                                    x is FeeBillingItem
                                )
                                .Sum(x => x.Amount.GetValue());

                            item.Adjustments = currentBilling.BillingItems
                                .Where(x => x is AdjustmentBillingItem)
                                .Sum(x => x.Amount.GetValue());

                            item.StatementDate = currentBilling.StatementDate;
                            item.DueDate = currentBilling.DueDate;
                        }

                        ViewModel.Items.Add(item);
                    }

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
