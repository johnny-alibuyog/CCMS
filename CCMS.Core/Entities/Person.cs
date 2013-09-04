using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class Person
    {
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private Nullable<DateTime> _birthDate;

        public virtual string FirstName 
        {
            get { return _firstName; }
        }

        public virtual string MiddleName 
        {
            get { return _middleName; }
        }

        public virtual string LastName
        {
            get { return _lastName; }
        }

        public virtual Nullable<DateTime> BirthDate
        {
            get { return _birthDate; }
        }

        public virtual Nullable<int> Age 
        { 
            get { return Person.ComputeAge(this.BirthDate); } 
        }

        public virtual string Fullname 
        { 
            get { return Person.GetFullName(this); } 
        }

        #region Constructor

        public Person() { }

        public Person(string firstName, string middleName, string lastName, Nullable<DateTime> birthDate = null)
        {
            _firstName = firstName;
            _middleName = middleName;
            _lastName = lastName;
            _birthDate = birthDate;
        }

        #endregion

        #region Methods

        public static string GetFullName(Person person)
        {
            return
                (!string.IsNullOrWhiteSpace(person.FirstName) ? person.FirstName : string.Empty) +
                (!string.IsNullOrWhiteSpace(person.MiddleName) ? " " + person.MiddleName : string.Empty) +
                (!string.IsNullOrWhiteSpace(person.LastName) ? " " + person.LastName : string.Empty);
        }

        public static Nullable<int> ComputeAge(Nullable<DateTime> birthDate)
        {
            if (birthDate == null || birthDate.HasValue == false)
                return null;

            //var span = DateTime.Today - birthDate;

            //return Convert.ToInt32(span.Value.TotalDays / 365);


            var now = DateTime.Now;
            var age = now.Year - birthDate.Value.Year;

            if (now.Month < birthDate.Value.Month || (now.Month == birthDate.Value.Month && now.Day < birthDate.Value.Day))
                age--;

            return age;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = !string.IsNullOrWhiteSpace(this.Fullname)
                    ? this.Fullname.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public override bool Equals(object obj)
        {
            var that = obj as Person;

            if (that == null)
                return false;

            if (string.IsNullOrWhiteSpace(that.Fullname) && string.IsNullOrWhiteSpace(this.Fullname))
                return object.ReferenceEquals(this, that);

            return that.Fullname.Equals(this.Fullname);
        }

        public static bool operator ==(Person x, Person y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Person x, Person y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
