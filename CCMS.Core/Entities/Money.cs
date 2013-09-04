using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Exceptions;

namespace CCMS.Core.Entities
{
    public class Money
    {
        private Currency _currency;
        private decimal _value;

        public virtual Currency Currency
        {
            get { return _currency; }
        }

        public virtual decimal Value
        {
            get { return _value; }
        }

        #region Constructors

        public Money() { }

        public Money(Currency currency) : this(currency, 0M) { }

        public Money(Currency currency, decimal value)
        {
            _currency = currency;
            _value = value;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Money;

            if (that == null)
                return false;

            if (that.Currency != this.Currency)
                return false;

            if (that.Value != this.Value)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                if (_hashCode == null)
                {
                    unchecked
                    {
                        _hashCode = 17;
                        _hashCode = _hashCode * 23 + (this.Currency != null ? this.Currency.GetHashCode() : 0);
                        _hashCode = _hashCode * 23 + (this.Value.GetHashCode());
                    }
                }

                return _hashCode.Value;
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Money x, Money y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Money x, Money y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Operators

        private static void MatchCurrency(Money x, Money y)
        {
            if (x == null || x.Currency == null)
                throw new ArgumentNullException("x");

            if (y == null || y.Currency == null)
                throw new ArgumentNullException("y");

            if (x.Currency != y.Currency)
                throw new BusinessException(string.Format("Mathematical operation is invalid for money with differnt currency: {0} - {1}.", x.Currency.Id, y.Currency.Id));
                //throw new InvalidOperationException(string.Format("Mathematical operation is invalid for money with differnt currency: {0} - {1}.", x.Currency.Id, y.Currency.Id));
        }

        public static Money operator +(Money x, Money y)
        {
            if (x == null)
                x = new Money(y.Currency);

            MatchCurrency(x, y);
            return new Money(x.Currency, x.Value + y.Value);
        }

        public static Money operator -(Money x, Money y)
        {
            if (x == null)
                x = new Money(y.Currency);

            MatchCurrency(x, y);
            return new Money(x.Currency, x.Value - y.Value);
        }

        public static Money operator *(Money x, Money y)
        {
            if (x == null)
                x = new Money(y.Currency);

            MatchCurrency(x, y);
            return new Money(x.Currency, x.Value * y.Value);
        }

        public static Money operator /(Money x, Money y)
        {
            if (x == null)
                x = new Money(y.Currency);

            MatchCurrency(x, y);
            return new Money(x.Currency, x.Value / y.Value);
        }

        public static bool operator >(Money x, Money y)
        {
            MatchCurrency(x, y);
            return x.Value > y.Value;
        }

        public static bool operator <(Money x, Money y)
        {
            MatchCurrency(x, y);
            return x.Value < y.Value;
        }

        public static bool operator >=(Money x, Money y)
        {
            MatchCurrency(x, y);
            return (x.Value > y.Value) || (x.Value == y.Value);
        }

        public static bool operator <=(Money x, Money y)
        {
            MatchCurrency(x, y);
            return (x.Value < y.Value) || (x.Value == y.Value);
        }

        public virtual Money Plus(decimal value)
        {
            return new Money(_currency, _value + value);
        }

        public virtual Money Minus(decimal value)
        {
            return new Money(_currency, _value - value);
        }

        public virtual Money Times(decimal value)
        {
            return new Money(_currency, _value * value);
        }

        public virtual Money DevidedBy(decimal value)
        {
            return new Money(_currency, _value / value);
        }

        #endregion

        #region Method Override

        public override string ToString()
        {
            return string.Format("{0} {1}",
                _value.ToString("N2"),
                _currency != null ? _currency.Id : string.Empty
            );
        }

        #endregion
    }
}
