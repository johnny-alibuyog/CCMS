using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Billings
{
    public class InstallmentBillingItemViewModel : BillingItemViewModel
    {
        #region Constructors

        public InstallmentBillingItemViewModel() : base(title: "Installment", type: "Installment") { }

        public InstallmentBillingItemViewModel(BillingItemViewModel value) : base(value) { } 

        #endregion
    }
}
