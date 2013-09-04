using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class PaymentItem
    {
        private Guid _id;
        private Payment _payment;
        private Billing _billing;
        private Money _amount;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual Payment Payment
        {
            get { return _payment; }
            set { _payment = value; }
        }

        public virtual Billing Billing
        {
            get { return _billing; }
            protected set { _billing = value; }
        }

        public virtual Money Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        #region Constructors

        public PaymentItem() { }

        public PaymentItem(Billing billing)
        {
            _billing = billing;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as PaymentItem;

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

        public static bool operator ==(PaymentItem x, PaymentItem y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(PaymentItem x, PaymentItem y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
