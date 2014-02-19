using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Reports
{
    public class AnnualSummaryController : ControllerBase<AnnualSummaryViewModel>
    {
        public AnnualSummaryController(AnnualSummaryViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Year = DateTime.Today.Year;

            this.PopulateLookup();

            this.ViewModel.Generate = new ReactiveCommand();
            this.ViewModel.Generate.Subscribe(x => Populate());

            this.ViewModel.WhenAny(x => x.SelectedCreditCard, x => true)
                .Subscribe(x => Populate());
        }

        private void PopulateLookup()
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<CreditCard>()
                        .Select(x => new
                        {
                            Key = x.Id,
                            Value = string.Format("[{0}] {1}", x.AccountNumber, x.Provider.Id)
                        })
                        .ToList();

                    items.Insert(0, new { Key = Guid.Empty, Value = "<< All >>" });

                    this.ViewModel.CreditCards = items.Select(x => new KeyValuePair<Guid, string>(x.Key, x.Value)).ToList();
                    this.ViewModel.SelectedCreditCard = this.ViewModel.CreditCards.FirstOrDefault();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Populate()
        {
            try
            {
                if (this.ViewModel.Render == null)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Billing>();

                    if (this.ViewModel.Year != 0)
                        query = query.Where(x => x.StatementDate.Year == this.ViewModel.Year);

                    if (this.ViewModel.SelectedCreditCard.Key != Guid.Empty)
                        query = query.Where(x => x.CreditCard.Id == this.ViewModel.SelectedCreditCard.Key);

                    this.ViewModel.Items = query
                        .Select(x => new AnnualSummaryItemViewModel()
                        {
                            Provider = x.CreditCard.Provider.Name,
                            Currency = x.CreditCard.TransactionCurrency.Name,
                            AccountName = x.CreditCard.AccountName,
                            AccountNumber = x.CreditCard.AccountNumber,
                            StatementDate = x.StatementDate,
                            PreviousBalance = x.PreviousBillingAmount.Value,
                            Payments = x.PaymentAmount.Value,
                            PurchasesInstallments = x.PurchaseAmount.Value + x.InstallmentAmount.Value,
                            CashAdvances = x.CashAdvanceAmount.Value,
                            InterestsFeesCharges = x.InterestAmount.Value + x.FeeAmount.Value + x.ChargeAmount.Value,
                            Adjustments = x.AdjustmentAmount.Value,
                            Balance = x.SettlementBalance.Value
                        })
                        .OrderBy(x => x.StatementDate)
                        .ToList();

                    transaction.Commit();
                }

                this.ViewModel.Render.Invoke();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }
    }
}
