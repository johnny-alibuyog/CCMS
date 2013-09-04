using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Billings
{
    public class FeeBillingItemViewModel : BillingItemViewModel
    {
        #region Constructors

        public FeeBillingItemViewModel() : base(title: "Fee", type: "Fee") { }

        public FeeBillingItemViewModel(BillingItemViewModel value) : base(value) { } 

        #endregion
    }
}
