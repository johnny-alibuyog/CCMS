using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Features.Reports;
using NHibernate;
using NHibernate.Linq;

namespace CCMS.UI.Features.Reports
{
    public class IncommingStatementSummaryController : ControllerBase<IncommingStatementSummaryViewModel>
    {
        public IncommingStatementSummaryController(IncommingStatementSummaryViewModel viewModel) : base(viewModel)
        {
            this.Populate();
        }

        public void Populate()
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Billing>()
                        .Where(x =>
                            x.DueDate > DateTime.Today &&
                            x.BillingStatus == BillingStatus.Unpaid
                        )
                        .Select(x => new IncommingStatementSummaryItemViewModel()
                        {
                            Provider = x.CreditCard.Provider.Name,
                            Currency = x.CreditCard.TransactionCurrency.Name,
                            AccountName = x.CreditCard.AccountName,
                            AccountNumber = x.CreditCard.AccountNumber,
                            ExpectedDueDate = x.DueDate,
                            ExpectedAmountDue = x.SettlementBalance.Value,
                        });

                    ViewModel.Items = new ObservableCollection<IncommingStatementSummaryItemViewModel>(query.ToList());

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
