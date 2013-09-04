using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class InstallmentBillingItem : BillingItem
    {
        private Installment _installment;

        public virtual Installment Installment
        {
            get { return _installment; }
            protected set { _installment = value; }
        }

        #region Abstract Overrides

        public override string GetTransactionClassifiaction()
        {
            var transactionClassification = this.Installment.TransactionClassification;
            return transactionClassification != null
                ? transactionClassification.Name
                : string.Empty;
        }

        public override string GetStaffName()
        {
            var staff = this.Installment.Staff;
            return staff != null
                ? staff.Person.Fullname
                : string.Empty;
        }

        #endregion

        #region Constructors

        public InstallmentBillingItem() { }

        public InstallmentBillingItem(Installment item, DateTime date)
        {
            _installment = item;
            this.Date = date;
            this.Details = item.Details; // string.Format("Installment: {0}", item.Details);
            this.Amount = item.Amortization;
        }

        #endregion
    }
}
