using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Billings
{
    public class InterestBillingItemViewModel : BillingItemViewModel
    {
        #region Constructors

        public InterestBillingItemViewModel() : base(title: "Interest", type: "Interest") { }

        public InterestBillingItemViewModel(BillingItemViewModel value) : base(value) { } 

        #endregion
    }
}
