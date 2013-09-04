using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class TransactionClassification
    {
        private Guid _id;
        private string _name;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //public static readonly string Others = "Others";

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as TransactionClassification;

            if (that == null)
                return false;

            if (that.Name != this.Name)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                unchecked
                {
                    _hashCode = 17;
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.Name) ? this.Name.GetHashCode() : 0);
                }
            }

            return _hashCode.Value;
        }

        public static bool operator ==(TransactionClassification x, TransactionClassification y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(TransactionClassification x, TransactionClassification y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
