using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Exceptions;
using CCMS.Core.Common.Extentions;

namespace CCMS.Core.Entities
{
    public class Payment
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Billing _billing;
        private DateTime _date;
        private TransactionClassification _transactionClassification;
        private Staff _staff;
        private Money _amount;

        public virtual Guid Id 
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual Audit Audit
        {
            get { return _audit; }
            set { _audit = value; }
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

        public virtual TransactionClassification TransactionClassification
        {
            get { return _transactionClassification; }
            set { _transactionClassification = value; }
        }

        public virtual Staff Staff
        {
            get { return _staff; }
            set { _staff = value; }
        }

        public virtual Money Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Purchase;

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

        public static bool operator ==(Payment x, Payment y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Payment x, Payment y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
