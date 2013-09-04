using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Users;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Staffs
{
    public class StaffViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        [NotNull(), Valid()]
        public virtual PersonViewModel Person { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public StaffViewModel()
        {
            this.Person = new PersonViewModel();
            this.WhenAny(x => x.Person.IsValid, x => x.Value)
                .Subscribe(x => this.IsValid = x);
        }

        internal void HydrateWith(StaffViewModel value)
        {
            this.Id = value.Id;
            this.Person.FirstName = value.Person.FirstName;
            this.Person.MiddleName = value.Person.MiddleName;
            this.Person.LastName = value.Person.LastName;
            this.Person.BirthDate = value.Person.BirthDate;
        }
    }
}
