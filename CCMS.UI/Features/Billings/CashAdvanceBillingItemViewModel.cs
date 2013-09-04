using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Billings
{
    public class CashAdvanceBillingItemViewModel : BillingItemViewModel
    {
        #region Constructors

        public CashAdvanceBillingItemViewModel() : base(title: "Cash Advance", type: "Cash Advance") { }

        public CashAdvanceBillingItemViewModel(BillingItemViewModel value) : base(value) { } 

        #endregion
    }
}
