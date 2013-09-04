using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class AdjustmentBillingItem : BillingItem
    {
        private Adjustment _adjustment;

        public virtual Adjustment Adjustment
        {
            get { return _adjustment; }
            protected set { _adjustment = value; }
        }

        #region Abstract Overrides

        public override string GetTransactionClassifiaction()
        {
            var transactionClassification = this.Adjustment.TransactionClassification;
            return transactionClassification != null
                ? transactionClassification.Name
                : string.Empty;
        }

        public override string GetStaffName()
        {
            var staff = this.Adjustment.Staff;
            return staff != null
                ? staff.Person.Fullname
                : string.Empty;
        }

        #endregion

        #region Constructors

        public AdjustmentBillingItem() { }

        public AdjustmentBillingItem(Adjustment item)
        {
            _adjustment = item;
            this.Date = item.Date;
            this.Details = item.Details; // string.Format("Adjustment: {0}", item.Details);
            this.Amount = item.Amount;
        }

        #endregion
    }
}
