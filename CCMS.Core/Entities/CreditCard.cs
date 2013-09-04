using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Extentions;

namespace CCMS.Core.Entities
{
    public class CreditCard
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private string _accountNumber;
        private string _accountName;
        private int _cutOff;
        private Nullable<DateTime> _issueDate;
        private Nullable<DateTime> _expiryDate;
        private User _user;
        private CreditCardProvider _provider;
        private Currency _transactionCurrency;
        private Money _creditLimit;
        private Money _cashAdvanceLimit;
        private Money _outstandingBalance;
        private Money _availableCredit;
        private ICollection<Billing> _billings;

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

        public virtual string AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; }
        }

        public virtual string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        public virtual int CutOff
        {
            get { return _cutOff; }
            set { _cutOff = value; }
        }

        public virtual Nullable<DateTime> IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; }
        }

        public virtual Nullable<DateTime> ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public virtual CreditCardProvider Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        public virtual Currency TransactionCurrency
        {
            get { return _transactionCurrency; }
            set { _transactionCurrency = value; }
        }

        public virtual Money CreditLimit
        {
            get { return _creditLimit; }
            set { _creditLimit = value; }
        }

        public virtual Money CashAdvanceLimit
        {
            get { return _cashAdvanceLimit; }
            set { _cashAdvanceLimit = value; }
        }

        public virtual Money OutstandingBalance
        {
            get { return _outstandingBalance; }
            set { _outstandingBalance = value; }
        }

        public virtual Money AvailableCredit
        {
            get { return _availableCredit; }
            set { _availableCredit = value; }
        }

        public virtual IEnumerable<Billing> Billings
        {
            get { return _billings; }
        }

        #region Methods

        public virtual Money ResolveMoney(decimal value)
        {
            return new Money(_transactionCurrency, value);
        }

        public virtual void Debit(Money amount)
        {
            if (_outstandingBalance == null)
                _outstandingBalance = new Money(_transactionCurrency);

            if (_availableCredit == null)
                _availableCredit = new Money(_transactionCurrency, _creditLimit.GetValue());

            _outstandingBalance += amount;
            _availableCredit -= amount;
        }

        public virtual void Credit(Money amount)
        {
            if (_outstandingBalance == null)
                _outstandingBalance = new Money(_transactionCurrency);

            if (_availableCredit == null)
                _availableCredit = new Money(_transactionCurrency, _creditLimit.GetValue());

            _outstandingBalance -= amount;
            _availableCredit += amount;
        }

        public virtual void AddBilling(Billing item)
        {
            item.CreditCard = this;
            _billings.Add(item);
        }

        public virtual void RemoveBilling(Billing item)
        {
            item.CreditCard = null;
            _billings.Remove(item);
        }

        public virtual Money Compute<T>(Money amount) where T : ComputationSetting
        {
            var computationSetting = _provider.ComputationSettings.OfType<T>()
                .Where(x =>
                    x.MinimumAmount != null &&
                    x.MinimumAmount.Currency.Id == amount.Currency.Id
                )
                .FirstOrDefault();

            if (computationSetting != null)
                return computationSetting.Compute(amount);

            return new Money(amount.Currency, 0M);
        }

        #endregion

        #region Constructors

        public CreditCard()
        {
            _billings = new List<Billing>();
        }

        public CreditCard(User user, CreditCardProvider provider, Currency transactionCurrency) : this()
        {
            _user = user;
            _provider = provider;
            _transactionCurrency = transactionCurrency;
            _creditLimit = new Money(_transactionCurrency);
            _cashAdvanceLimit = new Money(_transactionCurrency);
            _outstandingBalance = new Money(_transactionCurrency);
            _availableCredit = new Money(_transactionCurrency);
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as CreditCard;

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

        public static bool operator ==(CreditCard x, CreditCard y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(CreditCard x, CreditCard y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}