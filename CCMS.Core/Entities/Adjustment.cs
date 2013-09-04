using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class Adjustment
    {
        private Guid _id;
        private CreditCard _creditCard;
        private DateTime _date;
        private string _details;
        private TransactionClassification _transactionClassification;
        private Staff _staff;
        private Money _amount;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual CreditCard CreditCard
        {
            get { return _creditCard; }
            set { _creditCard = value; }
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
            var that = obj as Adjustment;

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

        public static bool operator ==(Adjustment x, Adjustment y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Adjustment x, Adjustment y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
