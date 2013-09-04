using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Users
{
    public class AuthenticationController : ControllerBase<AuthenticationViewModel>
    {
        public AuthenticationController(AuthenticationViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.CurrentViewModel = this.ViewModel.Login;

            this.ViewModel.ObservableForProperty(x => x.Login.ActionResult).Value()
                .Subscribe(x => this.ViewModel.ActionResult = x);

            this.ViewModel.ObservableForProperty(x => x.Registration.ActionResult).Value()
                .Subscribe(x => this.ViewModel.ActionResult = x);

            this.ViewModel.Toggle = new ReactiveCommand();
            this.ViewModel.Toggle.Subscribe(x =>
            {
                if (this.ViewModel.CurrentViewModel == this.ViewModel.Login)
                    this.ViewModel.CurrentViewModel = this.ViewModel.Registration;
                else
                    this.ViewModel.CurrentViewModel = this.ViewModel.Login;

            });
        }
    }
}
