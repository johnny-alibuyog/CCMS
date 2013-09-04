using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class Installment
    {
        private Guid _id;
        private CreditCard _creditCard;
        private DateTime _date;
        private string _details;
        private TransactionClassification _transactionClassification;
        private Staff _staff;
        private int _terms;
        private decimal _interestRate;
        private Money _amortization;
        private Money _interest;
        private Money _amount;
        private Money _balance;

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

        public virtual int Terms
        {
            get { return _terms; }
            set { _terms = value; }
        }

        public virtual decimal InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; }
        }

        public virtual Money Amortization
        {
            get { return _amortization; }
            set { _amortization = value; }
        }

        public virtual Money Interest
        {
            get { return _interest; }
            set { _interest = value; }
        }

        public virtual Money Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public virtual Money Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Installment;

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

        public static bool operator ==(Installment x, Installment y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Installment x, Installment y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
