using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Billings
{
    public class ChargeBillingItemViewModel : BillingItemViewModel
    {
        #region Constructors

        public ChargeBillingItemViewModel() : base(title: "Charge", type: "Charge") { }

        public ChargeBillingItemViewModel(BillingItemViewModel value) : base(value) { }

        #endregion
    }
}