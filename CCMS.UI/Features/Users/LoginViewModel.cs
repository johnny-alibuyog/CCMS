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
    public class LoginViewModel : ViewModelBase
    {
        private readonly LoginController _controller;

        [NotNullNotEmpty(Message = "Username is mandatory.")]
        public virtual string Username { get; set; }

        [NotNullNotEmpty(Message = "Password is mandatory.")]
        public virtual string Password { get; set; }

        public virtual IReactiveCommand Login { get; set; }

        public virtual IReactiveCommand Cancel { get; set; }

        public LoginViewModel()
        {
            _controller = new LoginController(this);
        }
    }
}
