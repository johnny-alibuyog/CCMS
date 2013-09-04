using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Extentions;

namespace CCMS.Core.Entities
{
    public class Billing
    {
        private Guid _id;
        private CreditCard _creditCard;
        private BillingStatus _billingStatus;
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _statementDate;
        private DateTime _dueDate;
        private Money _previousBillingAmount;
        private Money _previousPaymentAmount;
        private Money _adjustmentAmount;
        private Money _cashAdvanceAmount;
        private Money _chargeAmount;
        private Money _feeAmount;
        private Money _installmentAmount;
        private Money _interestAmount;
        private Money _purchaseAmount;
        private Money _billingAmount; /// total (adjustment + cash advance + charge + fee + installment + purchase + previous billing -  previous payment)
        private Money _paymentAmount;
        private Money _settlementBalance;
        private ICollection<Payment> _payments;
        private ICollection<BillingItem> _billingItems;
        private ICollection<Billing> _previousBillings;

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

        public virtual BillingStatus BillingStatus
        {
            get { return _billingStatus; }
            set { _billingStatus = value; }
        }

        public virtual DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public virtual DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public virtual DateTime StatementDate
        {
            get { return _statementDate; }
            set { _statementDate = value; }
        }

        public virtual DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        public virtual Money PreviousBillingAmount
        {
            get { return _previousBillingAmount; }
            //set { _previousBillingAmount = value; }
        }

        public virtual Money PreviousPaymentAmount
        {
            get { return _previousPaymentAmount; }
            //set { _previousPaymentAmount = value; }
        }

        public virtual Money AdjustmentAmount
        {
            get { return _adjustmentAmount; }
            //protected set { _adjustmentAmount = value; }
        }

        public virtual Money CashAdvanceAmount
        {
            get { return _cashAdvanceAmount; }
            //protected set { _cashAdvanceAmount = value; }
        }

        public virtual Money ChargeAmount
        {
            get { return _chargeAmount; }
            //protected set { _chargeAmount = value; }
        }

        public virtual Money FeeAmount
        {
            get { return _feeAmount; }
            //protected set { _feeAmount = value; }
        }

        public virtual Money InstallmentAmount
        {
            get { return _installmentAmount; }
            //protected set { _installmentAmount = value; }
        }

        public virtual Money InterestAmount
        {
            get { return _interestAmount; }
            //protected set { _interestAmount = value; }
        }

        public virtual Money PurchaseAmount
        {
            get { return _purchaseAmount; }
            //protected set { _purchaseAmount = value; }
        }

        public virtual Money BillingAmount
        {
            get { return _billingAmount; }
            //set { _billingAmount = value; }
        }

        public virtual Money PaymentAmount
        {
            get { return _paymentAmount; }
            //set { _paymentAmount = value; }
        }

        public virtual Money SettlementBalance
        {
            get { return _settlementBalance; }
            set
            {
                _settlementBalance = value;

                if (_billingStatus == BillingStatus.Merged)
                    return;

                if (_settlementBalance.GetValue() <= 0M)
                    _billingStatus = BillingStatus.Paid;
                else
                    _billingStatus = BillingStatus.Unpaid;
            }
        }

        public virtual ICollection<Payment> Payments
        {
            get { return _payments; }
        }

        public virtual IEnumerable<BillingItem> BillingItems
        {
            get { return _billingItems; }
        }

        public virtual IEnumerable<Billing> PreviousBillings
        {
            get { return _previousBillings; }
        }

        #region Constructors

        public Billing()
        {
            _payments = new Collection<Payment>();
            _billingItems = new Collection<BillingItem>();
            _previousBillings = new Collection<Billing>();
        }

        #endregion

        #region Methods

        public virtual void EnsureMoneyInitialition()
        {
            Func<Money> NewMoney = () => new Money(_creditCard.TransactionCurrency);

            if (_previousBillingAmount == null)
                _previousBillingAmount = NewMoney();

            if (_previousPaymentAmount == null)
                _previousPaymentAmount = NewMoney();

            if (_adjustmentAmount == null)
                _adjustmentAmount = NewMoney();

            if (_cashAdvanceAmount == null)
                _cashAdvanceAmount = NewMoney();

            if (_chargeAmount == null)
                _chargeAmount = NewMoney();

            if (_feeAmount == null)
                _feeAmount = NewMoney();

            if (_installmentAmount == null)
                _installmentAmount = NewMoney();

            if (_interestAmount == null)
                _interestAmount = NewMoney();

            if (_purchaseAmount == null)
                _purchaseAmount = NewMoney();

            if (_billingAmount == null)
                _billingAmount = NewMoney();

            if (_paymentAmount == null)
                _paymentAmount = NewMoney();

            if (_settlementBalance == null)
                _settlementBalance = NewMoney();
        }

        public virtual void SetDuration(DateTime date)
        {
            var referenceDate = CreditCard.CutOff >= date.Day
                ? new DateTime(date.Year, date.Month, CreditCard.CutOff).AddMonths(-1)
                : new DateTime(date.Year, date.Month, CreditCard.CutOff);

            _startDate = referenceDate;
            _endDate = referenceDate.AddMonths(1).AddTicks(-1);
            _statementDate = _endDate.AddDays(10);
            _dueDate = _endDate.AddDays(30);
        }

        public virtual void AddPayment(Payment item)
        {
            EnsureMoneyInitialition();

            _paymentAmount += item.Amount;
            _settlementBalance -= item.Amount;
            _creditCard.Credit(item.Amount);

            item.Billing = this;
            _payments.Add(item);

            EvaluateBilingStatus();
        }

        public virtual void RemovePayment(Payment item)
        {
            EnsureMoneyInitialition();

            _paymentAmount -= item.Amount;
            _settlementBalance += item.Amount;
            _creditCard.Debit(item.Amount);

            item.Billing = null;
            _payments.Remove(item);

            EvaluateBilingStatus();
        }

        public virtual void AddBillingItem(BillingItem item)
        {
            EnsureMoneyInitialition();

            if (item is AdjustmentBillingItem)
                _adjustmentAmount += item.Amount;
            else if (item is CashAdvanceBillingItem)
                _cashAdvanceAmount += item.Amount;
            else if (item is ChargeBillingItem)
                _chargeAmount += item.Amount;
            else if (item is FeeBillingItem)
                _feeAmount += item.Amount;
            else if (item is InstallmentBillingItem)
                _installmentAmount += item.Amount;
            else if (item is InterestBillingItem)
                _interestAmount += item.Amount;
            else if (item is PurchaseBillingItem)
                _purchaseAmount += item.Amount;

            _billingAmount += item.Amount;
            _settlementBalance += item.Amount;
            _creditCard.Debit(item.Amount);

            item.Billing = this;
            _billingItems.Add(item);

            EvaluateBilingStatus();
        }

        public virtual void RemoveBillingItem(BillingItem item)
        {
            EnsureMoneyInitialition();

            if (item is AdjustmentBillingItem)
                _adjustmentAmount -= item.Amount;
            else if (item is CashAdvanceBillingItem)
                _cashAdvanceAmount -= item.Amount;
            else if (item is ChargeBillingItem)
                _chargeAmount -= item.Amount;
            else if (item is FeeBillingItem)
                _feeAmount -= item.Amount;
            else if (item is InstallmentBillingItem)
                _installmentAmount -= item.Amount;
            else if (item is InterestBillingItem)
                _interestAmount -= item.Amount;
            else if (item is PurchaseBillingItem)
                _purchaseAmount -= item.Amount;

            _billingAmount -= item.Amount;
            _settlementBalance -= item.Amount;
            _creditCard.Credit(item.Amount);

            item.Billing = null;
            _billingItems.Remove(item);

            EvaluateBilingStatus();
        }

        public virtual void AddPreviousBilling(Billing item)
        {
            if (_previousBillings.Contains(item))
                return;

            EnsureMoneyInitialition();

            _previousBillingAmount += item.SettlementBalance;
            _previousPaymentAmount += item.PaymentAmount;

            _settlementBalance += item.SettlementBalance;
            _settlementBalance -= item.PaymentAmount;

            _billingAmount += item.SettlementBalance;
            _billingAmount -= item.PaymentAmount;

            _previousBillings.Add(item);

            item.BillingStatus = BillingStatus.Merged;

            EvaluateBilingStatus();
        }

        public virtual void RemovePreviousBilling(Billing item)
        {
            if (!_previousBillings.Contains(item))
                return;

            EnsureMoneyInitialition();

            _previousBillingAmount -= item.SettlementBalance;
            _previousPaymentAmount -= item.PaymentAmount;

            _settlementBalance -= item.SettlementBalance;
            _settlementBalance += item.PaymentAmount;

            _billingAmount -= item.SettlementBalance;
            _billingAmount += item.PaymentAmount;

            _previousBillings.Remove(item);

            item.BillingStatus = item.SettlementBalance.GetValue() > 0M
                ? BillingStatus.Unpaid
                : BillingStatus.Merged;

            EvaluateBilingStatus();
        }

        public virtual void MergeWith(IEnumerable<Billing> items)
        {
            foreach (var item in items)
                AddPreviousBilling(item);
        }

        public virtual void UnmergeWith(IEnumerable<Billing> items)
        {
            foreach (var item in items)
                RemovePreviousBilling(item);
        }

        public virtual Money GetPaymentsAndCredits()
        {
            return _previousPaymentAmount + _paymentAmount;
        }

        private void EvaluateBilingStatus()
        {
            if (_billingStatus == BillingStatus.Merged)
                return;

            if (_settlementBalance.GetValue() <= 0M)
                _billingStatus = BillingStatus.Paid;
            else
                _billingStatus = BillingStatus.Unpaid;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Billing;

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

        public static bool operator ==(Billing x, Billing y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Billing x, Billing y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
