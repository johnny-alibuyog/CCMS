using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Extentions;
using CCMS.Core.Entities;
using CCMS.UI.Features;
using CCMS.UI.Features.Contents;
using CCMS.UI.Features.CreditCards;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;

namespace CCMS.UI.Features.Billings
{
    public class CardTransactionsController : ControllerBase<CardTransactionsViewModel>
    {
        public CardTransactionsController(CardTransactionsViewModel viewModel)
            : base(viewModel)
        {
            this.MessageBus.Listen<CreditCardSelectedMessage>()
                .Subscribe(x => Populate(x.CreditCard.Id));

            this.MessageBus.Listen<RefreshMessage>()
                .Subscribe(x => Populate(App.Data.SelectedCreditCard.Id));
        }

        public void Populate(Guid creditCardId)
        {
            try
            {
                ViewModel.Items = new ObservableCollection<CardTransactionsItemViewModel>();

                // amount header
                this.ViewModel.AmountHeader = string.Format("Amount({0})",
                     App.Data.SelectedCreditCard != null
                        ? App.Data.SelectedCreditCard.TransactionCurrencyId
                        : string.Empty
                );

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var creditCardAlias = (CreditCard)null;
                    var billingAlias = (Billing)null;
                    var billingItemAlias = (BillingItem)null;

                    var billingQuery = session.QueryOver<Billing>(() => billingAlias)
                        .Left.JoinAlias(() => billingAlias.CreditCard, () => creditCardAlias)
                        .Left.JoinAlias(() => billingAlias.BillingItems, () => billingItemAlias)
                        .Where(() =>
                            creditCardAlias.Id == creditCardId &&
                            billingAlias.EndDate < DateTime.Today &&
                            billingAlias.BillingStatus == BillingStatus.Unpaid
                        )
                        .OrderBy(() => billingAlias.EndDate).Desc
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .Future();

                    var currentBilling = billingQuery.FirstOrDefault();
                    if (currentBilling == null)
                        return;

                    // merge previous to current billing
                    var previousBillings = billingQuery.Where(x => x != currentBilling);
                    currentBilling.MergeWith(previousBillings);

                    // previous balance
                    if (currentBilling.PreviousBillingAmount.GetValue() > 0M)
                    {
                        this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                        {
                            Details = "PREVIOUS BALANCE",
                            Amount = currentBilling.PreviousBillingAmount.GetValue(),
                        });
                    }

                    // payments/credits
                    if (currentBilling.PreviousPaymentAmount.GetValue() > 0M ||
                        currentBilling.PaymentAmount.GetValue() > 0M)
                    {
                        this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                        {
                            Details = "PAYMENTS - THANK YOU",
                            Amount = currentBilling.GetPaymentsAndCredits().GetValue(),
                        });
                    }

                    // current billing items
                    var billingItems = currentBilling.BillingItems.OrderBy(x => x.Date);
                    foreach (var item in billingItems)
                    {
                        this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                        {
                            Date = item.Date,
                            Details = item.Details,
                            TransactionClassification = item.GetTransactionClassifiaction(),
                            Staff = item.GetStaffName(),
                            Amount = item.Amount.Value
                        });
                    }

                    // total amount due
                    this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                    {
                        Details = "TOTAL AMOUNT DUE",
                        Amount = currentBilling.SettlementBalance.GetValue()
                    });

                    // end of statement
                    this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                    {
                        Details = "END OF THE STATEMENT"
                    });

                    transaction.Commit();

                    //var creditCardAlias = (CreditCard)null;
                    //var billingAlias = (Billing)null;
                    //var billingItemAlias = (BillingItem)null;

                    //var billings = session.QueryOver<Billing>(() => billingAlias)
                    //    .Left.JoinAlias(() => billingAlias.CreditCard, () => creditCardAlias)
                    //    .Left.JoinAlias(() => billingAlias.BillingItems, () => billingItemAlias)
                    //    .Where(() =>
                    //        creditCardAlias.Id == creditCardId &&
                    //        billingAlias.StartDate < DateTime.Today &&
                    //        billingAlias.BillingStatus == BillingStatus.Unpaid
                    //    )
                    //    .OrderBy(() => billingAlias.StartDate).Desc
                    //    .TransformUsing(Transformers.DistinctRootEntity)
                    //    .Future();

                    //var paymentQuery = session.QueryOver<Payment>()
                    //    .OrderBy(x => x.Date).Desc
                    //    .Left.JoinQueryOver(x => x.PaymentItems)
                    //    .Left.JoinQueryOver(x => x.Billing)
                    //    .TransformUsing(Transformers.DistinctRootEntity)
                    //    .Take(1)
                    //    .FutureValue();

                    //// previous payment
                    //var currentBilling = billings.FirstOrDefault();
                    //if (currentBilling == null)
                    //    return;

                    //this.ViewModel.AmountHeader = string.Format("Amount({0})",
                    //     App.Data.SelectedCreditCard != null
                    //        ? App.Data.SelectedCreditCard.TransactionCurrencyId
                    //        : string.Empty
                    //);

                    //// on-time
                    //if (billings.Count() == 1)
                    //{
                    //    var lastPayment = paymentQuery.Value;

                    //    // with previous payment
                    //    if (lastPayment != null && lastPayment.PaymentItems.Any(x => billings.Contains(x.Billing)))
                    //    {
                    //        this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                    //        {
                    //            Details = "PREVIOUS BALANCE",
                    //            Amount = lastPayment.PaymentItems.Sum(x => x.Billing.SettlementBalance.Value),
                    //        });

                    //        this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                    //        {
                    //            Date = lastPayment.Date,
                    //            Details = "PAYMENT - THANK YOU",
                    //            Amount = lastPayment.Amount.Value,
                    //        });
                    //    }
                    //}
                    //// over dued
                    //else
                    //{
                    //    var overduedBillings = billings.Where(x => x != currentBilling);
                    //    this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                    //    {
                    //        Details = "PREVIOUS BALANCE",
                    //        Amount = overduedBillings.Sum(x => x.SettlementBalance.Value),
                    //    });
                    //}

                    //// add items
                    //foreach (var item in currentBilling.BillingItems.OrderByDescending(x => x.Date))
                    //{
                    //    this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                    //    {
                    //        Date = item.Date,
                    //        Details = item.Details,
                    //        TransactionClassification = item.GetTransactionClassifiaction(),
                    //        Staff = item.GetStaffName(),
                    //        Amount = item.Amount.Value
                    //    });
                    //}

                    //this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                    //{
                    //    Details = "TOTAL AMOUNT DUE",
                    //    Amount = this.ViewModel.Items.Sum(x => x.Amount)
                    //});

                    //this.ViewModel.Items.Add(new CardTransactionsItemViewModel()
                    //{
                    //    Details = "END OF THE STATEMENT"
                    //});

                    //transaction.Commit();



                    //var currentBilling = query.FirstOrDefault();

                    //ViewModel.PreviousPayment = query
                    //    .Where(x => x != currentBilling)
                    //    .Sum(x => GetValue(x.PaymentAmount));

                    //ViewModel.PreviousBalance = query
                    //    .Where(x => x != currentBilling)
                    //    .Sum(x => GetValue(x.SettlementBalance));

                    //if (currentBilling != null)
                    //{
                    //    ViewModel.PurchaseInstallments = currentBilling.BillingItems
                    //        .Where(x =>
                    //            x is PurchaseBillingItem ||
                    //            x is InstallmentBillingItem
                    //        )
                    //        .Sum(x => GetValue(x.Amount));

                    //    ViewModel.CashAdvances = currentBilling.BillingItems
                    //        .Where(x => x is CashAdvanceBillingItem)
                    //        .Sum(x => GetValue(x.Amount));

                    //    ViewModel.InterestsFeesCharges = currentBilling.BillingItems
                    //        .Where(x =>
                    //            x is InterestBillingItem ||
                    //            x is ChargeBillingItem ||
                    //            x is FeeBillingItem
                    //        )
                    //        .Sum(x => GetValue(x.Amount));

                    //    ViewModel.Adjustments = currentBilling.BillingItems
                    //        .Where(x => x is AdjustmentBillingItem)
                    //        .Sum(x => GetValue(x.Amount));

                    //    ViewModel.StatementPeriodFrom = currentBilling.StartDate;
                    //    ViewModel.StatementPeriodTo = currentBilling.EndDate;
                    //}

                    //transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }
    }
}
