using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using System.ComponentModel;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Users
{
    public class AuthenticationViewModel : ViewModelBase
    {
        private readonly AuthenticationController _controller;

        public virtual ViewModelBase CurrentViewModel { get; set; }

        public virtual LoginViewModel Login { get; set; }

        public virtual RegistrationViewModel Registration { get; set; }

        public virtual string HeaderDisplayText
        {
            get { return (CurrentViewModel is LoginViewModel) ? "Login" : "Register"; }
        }

        public virtual string ToggleDisplayText
        {
            get { return (CurrentViewModel is LoginViewModel) ? "Register" : "Login"; }
        }

        public virtual IReactiveCommand Toggle { get; set; }


        public AuthenticationViewModel()
        {
            this.Login = new LoginViewModel();
            this.Registration = new RegistrationViewModel();

            _controller = new AuthenticationController(this);
        }

        //public AuthenticationViewModel(LoginViewModel login, RegistrationViewModel registration)
        //{
        //    this.Login = login;
        //    this.Registration = registration;

        //    //this.ObservableForProperty(x => x.CurrentViewModel)
        //    //    .Subscribe(x => 
        //    //    {
        //    //        this.raisePropertyChanged("HeaderDisplayText");
        //    //        this.raisePropertyChanged("ToggleDisplayText");
        //    //    });

        //    // initialize
        //    this.CurrentViewModel = this.Login;
        //    this.ObservableForProperty(x => x.Login.ActionResult)
        //        .ValueIfNotDefault()
        //        .Subscribe(x =>
        //        {
        //            this.ActionResult = x.Value;
        //        });

        //    this.ObservableForProperty(x => x.Registration.ActionResult)
        //        .ValueIfNotDefault()
        //        .Subscribe(x =>
        //        {
        //            this.ActionResult = x.Value;
        //        });

        //    this.Toggle = new RelayCommand(_ =>
        //    {
        //        if (this.CurrentViewModel == this.Login)
        //            this.CurrentViewModel = this.Registration;
        //        else
        //            this.CurrentViewModel = this.Login;

        //    });
        //}
    }
}
