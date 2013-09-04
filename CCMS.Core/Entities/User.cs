using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class User
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private string _username;
        private string _password;
        private Person _person;
        private ICollection<CreditCard> _creditCards;

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

        public virtual string Username 
        {
            get { return _username; }
            set { _username = value; }
        }

        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public virtual Person Person 
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual IEnumerable<CreditCard> CreditCards 
        {
            get { return _creditCards; }
        }

        #region Constructors

        public User()
        {
            _creditCards = new List<CreditCard>();
        }

        #endregion

        #region Methods

        public virtual void AddCreditCard(CreditCard item)
        {
            item.User = this;
            _creditCards.Add(item);
        }

        public virtual void AddCreditCards(IEnumerable<CreditCard> items)
        {
            foreach (var item in items)
                this.AddCreditCard(item);
        }

        public virtual void RemoveCreditCard(CreditCard item)
        {
            item.User = null;
            _creditCards.Remove(item);
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as User;

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

        public static bool operator ==(User x, User y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(User x, User y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
