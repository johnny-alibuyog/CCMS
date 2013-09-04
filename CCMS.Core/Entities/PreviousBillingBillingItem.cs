using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class PreviousBillingBillingItem : BillingItem
    {
        private Billing _previousBilliing;

        public virtual Billing PreviousBilling
        {
            get { return _previousBilliing; }
            protected set { _previousBilliing = value; }
        }

        #region Constructors

        public PreviousBillingBillingItem() { }

        public PreviousBillingBillingItem(Billing item)
        {
            _previousBilliing = item;
            this.Date = item.DueDate;
            this.Details = string.Format("Previous billing due {0}.", item.DueDate.ToString("yyyy-MM-dd"));
            this.Amount = item.SettlementBalance;
        }

        #endregion

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
