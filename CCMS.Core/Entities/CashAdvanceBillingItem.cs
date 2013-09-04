using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class CashAdvanceBillingItem : BillingItem
    {
        private CashAdvance _cashAdvance;

        public virtual CashAdvance CashAdvance
        {
            get { return _cashAdvance; }
            protected set { _cashAdvance = value; }
        }

        #region Abstract Overrides

        public override string GetTransactionClassifiaction()
        {
            var transactionClassification = this.CashAdvance.TransactionClassification;
            return transactionClassification != null
                ? transactionClassification.Name
                : string.Empty;
        }

        public override string GetStaffName()
        {
            var staff = this.CashAdvance.Staff;
            return staff != null
                ? staff.Person.Fullname
                : string.Empty;
        }

        #endregion


        #region Constructors

        public CashAdvanceBillingItem() { }

        public CashAdvanceBillingItem(CashAdvance item)
        {
            _cashAdvance = item;
            this.Date = item.Date;
            this.Details = item.Details; //string.Format("Cash Advance");
            this.Amount = item.Amount;
        }

        #endregion
    }
}
