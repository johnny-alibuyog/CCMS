using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public abstract class ComputationSetting
    {
        private Guid _id;
        private CreditCardProvider _provider;
        private Money _minimumAmount;
        private decimal _rate;

        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual CreditCardProvider Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        public virtual Money MinimumAmount
        {
            get { return _minimumAmount; }
            set { _minimumAmount = value; }
        }

        public virtual decimal Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        #region Method

        public virtual Money Compute(Money amount)
        {
            var percentAmount = amount.Times(_rate);
            return _minimumAmount > percentAmount
                ? _minimumAmount : percentAmount;
        }

        public virtual void SerializeWith(ComputationSetting value)
        {
            this.Id = value.Id;
            this.Provider = value.Provider;
            this.MinimumAmount = value.MinimumAmount;
            this.Rate = value.Rate;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as ComputationSetting;

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

        public static bool operator ==(ComputationSetting x, ComputationSetting y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(ComputationSetting x, ComputationSetting y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
