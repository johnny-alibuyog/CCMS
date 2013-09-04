using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Billings
{
    public class PurchaseBillingItemViewModel : BillingItemViewModel
    {
        #region Constructors

        public PurchaseBillingItemViewModel() : base(title: "Purchase", type: "Purchase") { }

        public PurchaseBillingItemViewModel(BillingItemViewModel value) : base(value) { } 

        #endregion
    }
}
