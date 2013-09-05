using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features.Users;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Users
{
    public class LoginController : ControllerBase<LoginViewModel>
    {
        public LoginController(LoginViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Login = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Login.Subscribe(x => Login());

            this.ViewModel.Cancel = new ReactiveCommand();
            this.ViewModel.Cancel.Subscribe(x => Cancel());
        }

        public Nullable<bool> Login()
        {
            try
            {
                var user = (UserViewModel)null;

                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    user = session.Query<User>()
                        .Where(x =>
                            x.Username == ViewModel.Username &&
                            x.Password == ViewModel.Password
                        )
                        .Select(x => new UserViewModel()
                        {
                            Id = x.Id,
                            Username = x.Username,
                            Fullname = x.Person.FirstName + " " + x.Person.LastName
                        })
                        .SingleOrDefault();

                    transaction.Commit();
                }

                if (user == null)
                {
                    this.MessageBox.Inform("Invalid login credentials.", "Login");
                    return null;
                }

                App.Data.CurrentUser = user;

                //ViewModel.ActionResult = (user != null) ? new Nullable<bool>(true) : null;
                this.ViewModel.Close(result: true);
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
                this.ViewModel.Close(result: false);
            }

            return ViewModel.ActionResult;
        }

        public virtual void Cancel()
        {
            this.ViewModel.Close(result: false);
        }
    }
}
