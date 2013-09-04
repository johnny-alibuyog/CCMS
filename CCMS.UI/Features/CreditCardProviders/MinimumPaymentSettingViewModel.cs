using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class MinimumPaymentSettingViewModel: ComputationSettingViewModel
    {
        #region Constructors

        public MinimumPaymentSettingViewModel() 
            : base(title: "Minimum Payment Setting", details: "Minimum Payment Setting") { } 

        #endregion
    }
}
