using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Extentions;
using CCMS.Core.Entities;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCards;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;

namespace CCMS.UI.Features.Summaries
{
    public class SummaryController : ControllerBase<SummaryViewModel>
    {
        public SummaryController(SummaryViewModel viewModel)
            : base(viewModel)
        {
            this.InitializeView();

            this.MessageBus.Listen<RefreshMessage>()
                .Subscribe(x => Populate(App.Data.SelectedCreditCard.Id));

            this.MessageBus.Listen<CreditCardSelectedMessage>()
                .Subscribe(x => Populate(x.CreditCard.Id));
        }

        public virtual void InitializeView()
        {
            if (App.Data.SelectedCreditCard != null)
            {
                var today = DateTime.Today;
                var referenceDate = App.Data.SelectedCreditCard.CuttOff >= today.Day
                    ? new DateTime(today.Year, today.Month, App.Data.SelectedCreditCard.CuttOff).AddMonths(-1)
                    : new DateTime(today.Year, today.Month, App.Data.SelectedCreditCard.CuttOff);

                this.ViewModel.StatementPeriodFrom = referenceDate;
                this.ViewModel.StatementPeriodTo = referenceDate.AddMonths(1).AddMilliseconds(-1);
            }

            this.ViewModel.PreviousBalance = 0M;
            this.ViewModel.PaymentsCredits = 0M;
            this.ViewModel.PurchasesInstallments = 0M;
            this.ViewModel.CashAdvances = 0M;
            this.ViewModel.InterestsFeesCharges = 0M;
        }

        public void Populate(Guid creditCardId)
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var billingQuery = session.QueryOver<Billing>()
                        .Where(x =>
                            x.EndDate < DateTime.Today &&
                            x.CreditCard.Id == creditCardId &&
                            x.BillingStatus == BillingStatus.Unpaid
                        )
                        .OrderBy(x => x.EndDate).Desc
                        .Fetch(x => x.BillingItems).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .Future();

                    this.InitializeView();

                    var currentBilling = billingQuery.FirstOrDefault();
                    if (currentBilling == null)
                        return;

                    // merge previous to current billing
                    var previousBillings = billingQuery.Where(x => x != currentBilling);
                    currentBilling.MergeWith(previousBillings);

                    // previous balance
                    ViewModel.PreviousBalance = currentBilling.PreviousBillingAmount.GetValue();

                    // payments credits
                    ViewModel.PaymentsCredits = currentBilling.GetPaymentsAndCredits().GetValue();

                    // purcahse and installments
                    ViewModel.PurchasesInstallments = (currentBilling.PurchaseAmount + currentBilling.InstallmentAmount).GetValue();

                    // cash advances
                    ViewModel.CashAdvances = currentBilling.CashAdvanceAmount.GetValue();

                    // interest / fees / charges
                    ViewModel.InterestsFeesCharges = (currentBilling.InterestAmount + currentBilling.ChargeAmount + currentBilling.FeeAmount).GetValue();

                    // adjustments
                    ViewModel.Adjustments = currentBilling.AdjustmentAmount.GetValue();

                    // statement period from
                    ViewModel.StatementPeriodFrom = currentBilling.StartDate;

                    // statement period to
                    ViewModel.StatementPeriodTo = currentBilling.EndDate;

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        //public void Populate(Guid creditCardId)
        //{
        //    using (var session = this.SessionFactory.OpenSession())
        //    using (var transaction = session.BeginTransaction())
        //    {
        //        var billingQuery = session.QueryOver<Billing>()
        //            .Where(x =>
        //                x.EndDate < DateTime.Today &&
        //                x.CreditCard.Id == creditCardId &&
        //                x.BillingStatus == BillingStatus.Unpaid
        //            )
        //            .OrderBy(x => x.EndDate).Desc
        //            .Fetch(x => x.BillingItems).Eager
        //            .TransformUsing(Transformers.DistinctRootEntity)
        //            .Future();

        //        this.InitializeView();

        //        var currentBilling = billingQuery.FirstOrDefault();
        //        if (currentBilling == null)
        //            return;

        //        // merge previous to current billing
        //        var previousBillings = billingQuery.Where(x => x != currentBilling);
        //        currentBilling.MergeWith(previousBillings);

        //        // previous balance
        //        ViewModel.PreviousBalance = currentBilling.PreviousBillingAmount.GetValue();

        //        // payments credits
        //        ViewModel.PaymentsCredits = currentBilling.GetPaymentsAndCredits().GetValue();

        //        // purcahse and installments
        //        ViewModel.PurchasesInstallments = currentBilling.BillingItems
        //            .Where(x =>
        //                x is PurchaseBillingItem ||
        //                x is InstallmentBillingItem
        //            )
        //            .Sum(x => x.Amount.GetValue());

        //        // cash advances
        //        ViewModel.CashAdvances = currentBilling.BillingItems
        //            .Where(x => x is CashAdvanceBillingItem)
        //            .Sum(x => x.Amount.GetValue());

        //        // interest / fees / charges
        //        ViewModel.InterestsFeesCharges = currentBilling.BillingItems
        //            .Where(x =>
        //                x is InterestBillingItem ||
        //                x is ChargeBillingItem ||
        //                x is FeeBillingItem
        //            )
        //            .Sum(x => x.Amount.GetValue());

        //        // adjustments
        //        ViewModel.Adjustments = currentBilling.BillingItems
        //            .Where(x => x is AdjustmentBillingItem)
        //            .Sum(x => x.Amount.GetValue());

        //        // statement period from
        //        ViewModel.StatementPeriodFrom = currentBilling.StartDate;

        //        // statement period to
        //        ViewModel.StatementPeriodTo = currentBilling.EndDate;

        //        transaction.Commit();
        //    }
        //}
    }
}
