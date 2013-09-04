using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Billings;
using ReactiveUI;

namespace CCMS.UI.Features.Summaries
{
    public class SummaryViewModel : ViewModelBase
    {
        private readonly SummaryController _controller;

        public virtual decimal PreviousBalance { get; set; }

        public virtual decimal PaymentsCredits { get; set; }

        public virtual decimal PurchasesInstallments { get; set; }

        public virtual decimal CashAdvances { get; set; }

        public virtual decimal InterestsFeesCharges { get; set; }

        public virtual decimal Adjustments { get; set; }

        public virtual decimal TotalAmountDue
        {
            get
            {
                return PreviousBalance + PurchasesInstallments + CashAdvances +
                    InterestsFeesCharges + Adjustments - PaymentsCredits;
            }
        }

        public virtual DateTime StatementPeriodFrom { get; set; }

        public virtual DateTime StatementPeriodTo { get; set; }

        public SummaryViewModel()
        {
            _controller = new SummaryController(this);
        }
    }
}
