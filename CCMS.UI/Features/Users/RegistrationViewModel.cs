using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Features;
using CCMS.UI.Features.Users;
using CCMS.UI.Infrastructure;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Users
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly RegistrationController _controller;
        
        [NotNullNotEmpty(Message = "Username is mandatory.")]
        public virtual string Username { get; set; }

        [NotNullNotEmpty(Message = "Password is mandatory.")]
        public virtual string Password { get; set; }

        [NotNullNotEmpty(Message = "Password confirmation is mandatory.")]
        public virtual string PasswordConfirmation { get; set; }

        [NotNull(), Valid()]
        public virtual PersonViewModel Person { get; set; }

        public virtual IReactiveCommand Register { get; set; }

        public virtual IReactiveCommand Cancel { get; set; }

        public RegistrationViewModel()
        {
            this.Person = new PersonViewModel();

            _controller = new RegistrationController(this);
        }
    }
}
