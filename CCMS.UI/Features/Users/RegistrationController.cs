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
    public class RegistrationController : ControllerBase<RegistrationViewModel>
    {
        public RegistrationController(RegistrationViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Register = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Register.Subscribe(x => Register());

            this.ViewModel.Cancel = new ReactiveCommand();
            this.ViewModel.Cancel.Subscribe(x => Cancel());

        }

        public Nullable<bool> Register()
        {
            try
            {
                var userViewModel = (UserViewModel)null;

                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<User>().Any(x => x.Username == this.ViewModel.Username);

                    User user = null;

                    if (!exists)
                    {
                        user = new User()
                        {
                            Username = this.ViewModel.Username,
                            Password = this.ViewModel.Password,
                            Person = new Person(
                                firstName: this.ViewModel.Person.FirstName,
                                middleName: this.ViewModel.Person.MiddleName,
                                lastName: this.ViewModel.Person.LastName,
                                birthDate: this.ViewModel.Person.BirthDate
                            ),
                        };

                        session.Insert(user);
                    }

                    transaction.Commit();

                    if (user == null)
                    {
                        this.MessageBox.Inform("Username already in use.", "Login");
                        return null;
                    }

                    userViewModel = new UserViewModel()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Fullname = user.Person.Fullname,
                    };
                }

                App.Data.CurrentUser = userViewModel;

                //ViewModel.ActionResult = (userViewModel != null) ? new Nullable<bool>(true) : null;
                this.ViewModel.Close(result: true);
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
                this.ViewModel.Close(result: false);
            }

            return this.ViewModel.ActionResult;
        }

        public virtual void Cancel()
        {
            this.ViewModel.Close(result: false);
        }
    }
}
