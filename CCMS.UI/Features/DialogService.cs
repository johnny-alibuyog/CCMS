using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.SplashScreens;

namespace CCMS.UI.Features
{
    public abstract class DialogService<TView, TViewModel> // : IDialogService<TView, TViewModel>
        where TView : Window
        where TViewModel : ViewModelBase
    {
        #region IDialogService<TView, TViewModel> Members

        public virtual TView View 
        {
            get; 
            private set; 
        }

        public virtual TViewModel ViewModel
        {
            get { return View.DataContext as TViewModel; }
            set { View.DataContext = value; }
        }

        public virtual void Show()
        {
            this.Show(null, null);
        }

        public virtual void Show(object sender, params object[] args)
        {
            View.Show();
        }

        public virtual void Close()
        {
            View.Close();
        }

        public virtual TViewModel ShowModal()
        {
            return this.ShowModal(null, null);
        }

        public virtual TViewModel ShowModal(object sender, string title, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(title))
                this.View.Title = title;

            if (args != null)
            {
                var viewModelInstance = args.OfType<TViewModel>().FirstOrDefault();
                if (viewModelInstance != null)
                    this.ViewModel = viewModelInstance;
            }

            this.ViewModel.Initialize();

            var result = View.ShowDialog();
            if (result != null && result == true)
                return this.ViewModel;
            else
                return null;
        }

        #endregion

        #region Constructos

        public DialogService(TView view, TViewModel viewModel)
        {
            //this.View = IoC.Container.Resolve<TView>();
            //if (this.View.DataContext == null)
            //    this.View.DataContext = IoC.Container.Resolve<TViewModel>();

            this.View = view;
            this.ViewModel = viewModel;

            // assign owner if View is not the main window
            if ((!(this.View is MainView) && !(this.View is SplashScreenView)))
                this.View.Owner = App.CurrentWindow;
        }

        #endregion
    }
}
