using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class PurchaseBillingItem : BillingItem
    {
        private Purchase _purchase;

        public virtual Purchase Purchase
        {
            get { return _purchase; }
            protected set { _purchase = value; }
        }

        #region Abstract Overrides

        public override string GetTransactionClassifiaction()
        {
            var transactionClassification = this.Purchase.TransactionClassification;
            return transactionClassification != null
                ? transactionClassification.Name
                : string.Empty;
        }

        public override string GetStaffName()
        {
            var staff    = this.Purchase.Staff;
            return staff != null
                ? staff.Person.Fullname
                : string.Empty;
        }

        #endregion

        #region Constructors

        public PurchaseBillingItem() { }

        public PurchaseBillingItem(Purchase item)
        {
            _purchase = item;
            this.Date = item.Date;
            this.Details = item.Details; // string.Format("Purchase: {0}", item.Details);
            this.Amount = item.Amount;
        }

        #endregion
    }
}
