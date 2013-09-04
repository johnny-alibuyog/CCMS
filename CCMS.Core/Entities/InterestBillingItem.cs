using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class InterestBillingItem : BillingItem
    {
        #region Abstract Overrides

        public override string GetTransactionClassifiaction()
        {
            return string.Empty;
        }

        public override string GetStaffName()
        {
            return string.Empty;
        }

        #endregion
    }
}
