using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Domain.Models
{
    public class PaymentStatus
    {
        private string _id;
        private string _name;

        public virtual string Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        #region Constructors

        public PaymentStatus() { }

        public PaymentStatus(string id, string name)
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Static Members

        public static PaymentStatus Unpaid = new PaymentStatus("UN", "Unpaid");
        public static PaymentStatus PartialyPaid = new PaymentStatus("PP", "PartialyPaid");
        public static PaymentStatus Paid = new PaymentStatus("PD", "Paid");
        public static PaymentStatus Recomputed = new PaymentStatus("RC", "Recomputed");

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as PaymentStatus;

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

        public static bool operator ==(PaymentStatus x, PaymentStatus y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(PaymentStatus x, PaymentStatus y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
