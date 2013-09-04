using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Reports
{
    public class AnnualSummaryItemViewModel
    {
        public virtual string Provider { get; set; }

        public virtual string Currency { get; set; }

        public virtual string AccountName { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual DateTime StatementDate { get; set; }

        public virtual decimal PreviousBalance { get; set; }

        public virtual decimal Payments { get; set; }

        public virtual decimal PurchasesInstallments { get; set; }

        public virtual decimal CashAdvances { get; set; }

        public virtual decimal InterestsFeesCharges { get; set; }

        public virtual decimal Adjustments { get; set; }

        public virtual decimal Balance { get; set; }

        //public virtual decimal Expences
        //{
        //    get { return PurchasesInstallments + CashAdvances + InterestsFeesCharges + Adjustments; }
        //}

        //public virtual decimal Balance
        //{
        //    get { return Expences - Balance; }
        //}
    }
}
