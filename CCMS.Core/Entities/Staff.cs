using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class Staff
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Person _person;
        private User _user;

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

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }

        #region Methods

        public virtual void SerializeWith(Staff value)
        {
            this.Person = value.Person;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Staff;

            if (that == null)
                return false;

            if (that.Person != this.Person)
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
                    _hashCode = _hashCode * 23 + (this.Person != null ? this.Person.GetHashCode() : 0);
                }
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Staff x, Staff y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Staff x, Staff y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
