using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Constraints;

namespace CCMS.UI.Features.Users
{
    public class PersonViewModel : ViewModelBase
    {
        [NotNullNotEmpty(Message = "Fisrt name is mandatory.")]
        public virtual string FirstName { get; set; }

        public virtual string MiddleName { get; set; }

        [NotNullNotEmpty(Message = "Last name is mandatory.")]
        public virtual string LastName { get; set; }

        [Past(Message = "Birth date sould be in the past.")]
        public virtual Nullable<DateTime> BirthDate { get; set; }

        public virtual string FullName
        {
            get 
            { 
                return string.Format("{0} {1} {2}", 
                    this.FirstName ?? string.Empty,
                    this.MiddleName ?? string.Empty,
                    this.LastName ?? string.Empty
                ); 
            }
        }

        public PersonViewModel()
        {
            //this.BirthDate = DateTime.Today;
        }
    }
}
