using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Infrastructure;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Currencies
{
    public class CurrencyViewModel : ViewModelBase
    {
        [NotNullNotEmpty]
        public virtual string Id { get; set; }

        [NotNullNotEmpty]
        public virtual string Name { get; set; }

        public virtual bool IsEditMode { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public virtual void HydrateWith(CurrencyViewModel value)
        {
            this.Id = value.Id;
            this.Name = value.Name;
            this.IsEditMode = true;
        }
    }
}
