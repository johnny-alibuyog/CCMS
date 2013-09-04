using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public abstract class BillingItem
    {
        private Guid _id;
        private Billing _billing;
        private DateTime _date;
        private string _details;
        private Money _amount;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual Billing Billing
        {
            get { return _billing; }
            set { _billing = value; }
        }

        public virtual DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual string Details
        {
            get { return _details; }
            set { _details = value; }
        }

        public virtual Money Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        #region Abstract Members

        public abstract string GetTransactionClassifiaction();

        public abstract string GetStaffName();

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as BillingItem;

            if (that == null)
                return false;

            if (that.Id == Guid.Empty && this.Id == Guid.Empty)
                return object.ReferenceEquals(that, this);

            return that.Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (this.Id != Guid.Empty)
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(BillingItem x, BillingItem y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BillingItem x, BillingItem y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
