using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Billings
{
    public class AdjustmentBillingItemViewModel : BillingItemViewModel 
    {
        #region Constructors

        public AdjustmentBillingItemViewModel() : base(title: "Adjustment", type: "Adjustment") { }

        public AdjustmentBillingItemViewModel(BillingItemViewModel value) : base(value) { } 

        #endregion
    }
}
