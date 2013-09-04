using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class Currency
    {
        private string _id;
        private string _name;

        public virtual string Id 
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name 
        {
            get { return _name; }
            set { _name = value; }
        }

        #region Constructors

        public Currency() { }

        public Currency(string id, string name)
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Static Members

        public static readonly Currency PHP = new Currency("PHP", "Philippine Peso");
        public static readonly Currency USD = new Currency("USD", "US Dollars");
        public static readonly IEnumerable<Currency> All = new Currency[] 
        { 
            Currency.PHP, 
            Currency.USD, 
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Currency;

            if (that == null)
                return false;

            if (string.IsNullOrWhiteSpace(that.Id) && string.IsNullOrWhiteSpace(this.Id))
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (!string.IsNullOrWhiteSpace(this.Id))
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Currency x, Currency y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Currency x, Currency y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Method Override

        public override string ToString()
        {
            return _id;
        }

        #endregion
    }
}
