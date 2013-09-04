using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;

namespace CCMS.UI.Features.CreditCards
{
    public class CreditCardInfoViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual string AccountName { get; set; }

        public virtual string Provider { get; set; }

        public virtual decimal InterestRate { get; set; }

        public virtual int CuttOff { get; set; }

        public virtual string TransactionCurrencyId { get; set; }

        public virtual decimal CreditLimit { get; set; }

        public virtual decimal CashAdvanceLimit { get; set; }

        public virtual string DisplayText
        {
            get { return string.Format("{0} [{1}]", Provider, AccountNumber); }
        }

        public override string ToString()
        {
            return DisplayText;
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as CreditCardInfoViewModel;

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

        public static bool operator ==(CreditCardInfoViewModel x, CreditCardInfoViewModel y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(CreditCardInfoViewModel x, CreditCardInfoViewModel y)
        {
            return !Equals(x, y);
        }

        #endregion

    }
}
