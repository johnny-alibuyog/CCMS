using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Reports
{
    public class StatementSummaryItemViewModel : ViewModelBase
    {
        #region Properties

        public virtual string Provider { get; set; }

        public virtual string Currency { get; set; }

        public virtual string AccountName { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual decimal CreditLimit { get; set; }

        public virtual decimal AvailableCredit { get; set; }

        public virtual decimal PreviousBalance { get; set; }

        public virtual decimal PaymentsCredits { get; set; }

        public virtual decimal PurchasesInstallments { get; set; }

        public virtual decimal CashAdvances { get; set; }

        public virtual decimal InterestsFeesCharges { get; set; }

        public virtual decimal Adjustments { get; set; }

        public virtual DateTime StatementDate { get; set; }

        public virtual DateTime DueDate { get; set; }

        public virtual decimal TotalAmountDue
        {
            get
            {
                return PreviousBalance - PaymentsCredits + PurchasesInstallments +
                    CashAdvances + InterestsFeesCharges + Adjustments;
            }
        }

        #endregion
    }
}
