using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CCMS.UI.Bootstrappers.DataInitializers;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Bootstrappers.Log;
using CCMS.UI.Bootstrappers.Metadata;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Navigations;
using CCMS.UI.Features.SplashScreens;
using CCMS.UI.Features.Users;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Global Variables

        private static AppViewModel _data;

        public static AppViewModel Data
        {
            get 
            {
                if (_data == null)
                    _data = new AppViewModel();

                return _data; 
            }
        }

        public static Window CurrentWindow
        {
            get
            {
                return Application.Current.Windows
                    .OfType<Window>()
                    .Where(x => x.IsActive)
                    .SingleOrDefault();
            }
        }

        #endregion

        private Mutex _instanceMutex = null;

        /// <summary>
        /// initialize all bootstrappers
        /// </summary>
        private void InitializeBootsrappers()
        {
            IoCBootstrapper.Initialize();
            LogBootstrapper.Initialize();
            MetadataBootstrapper.Initialize();
            DataBootstrapper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var createdNew = false;
            _instanceMutex = new Mutex(true, @"Credit Card Management System", out createdNew);
            if (!createdNew)
            {
                _instanceMutex = null;
                MessageBox.Show("Application is already running...", "Credit Card Management System", MessageBoxButton.OK);
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            this.InitializeBootsrappers();

            this.OnStartupExtracted();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_instanceMutex != null)
                _instanceMutex.ReleaseMutex();

            base.OnExit(e);
        }

        private void OnStartupExtracted()
        {
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            TestData.GenerateUser(persistOnCall: true);

            // authentication (login / registration)
            var authDialog = IoC.Container.Resolve<AuthenticationDialogService>();
            authDialog.ViewModel.Login.Username = "johnny.alibuyog";
            authDialog.ViewModel.Login.Password = "slow_dance";
            authDialog.ShowModal();
            var actionResult = authDialog.ViewModel.ActionResult;
            if (actionResult == null || actionResult.Value == false)
            {
                this.Shutdown(1);
                return;
            }

            // splash screen
            var splashDialog = IoC.Container.Resolve<SplashScreenDialogService>();
            splashDialog.ViewModel.Licensee = "Raquel Octera Corp";
            splashDialog.ViewModel.Plugins = new[] { "NHibernate", "Ninject", "ReactiveUI" };
            splashDialog.Show();

            // loading
            Task.Factory.StartNew(() =>
            {
                // generate data
                TestData.GenerateData();
            })
            .ContinueWith(task =>
            {
                var mainDialog = IoC.Container.Resolve<MainDialogService>();
                this.MainWindow = mainDialog.View;
                this.MainWindow.Loaded += (sender, args) => splashDialog.Close();
                this.MainWindow.ShowDialog();
                this.Shutdown(1);
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        //var splashScreenDialog = IoC.Container.Resolve<SplashScreenDialogService>();
        //splashScreenDialog.ViewModel.Licensee = "Raquel Octera Corp";
        //splashScreenDialog.ViewModel.Plugins = new[] { "NHibernate", "Ninject", "log4net" };
        //splashScreenDialog.View.Show();

        //// splash screen loading
        //splashScreenDialog.ViewModel.Message = "generating data ...";
        //Thread.Sleep(500);
        //TestData.Generate();

        //// loading ...
        //splashScreenDialog.ViewModel.Message = "loading something ...";
        //Thread.Sleep(500);

        //// loading ...
        //splashScreenDialog.ViewModel.Message = "loading something more ...";
        //Thread.Sleep(500);

        //// loading ...
        //splashScreenDialog.ViewModel.Message = "loading something more and more ...";
        //Thread.Sleep(500);

        //// loading ...
        //splashScreenDialog.ViewModel.Message = "finished loading ...";
        //Thread.Sleep(500);

        ////splashScreenDialog.View.Close();

        //// show main
        //var mainDialog = IoC.Container.Resolve<MainDialogService>();

        ////set application main window
        //this.MainWindow = mainDialog.View;
        //this.MainWindow.Loaded += (sender, args) => splashScreenDialog.View.Close();
        //this.MainWindow.Show();
        ////mainDialog.Show();

        //Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        //Application.Current.MainWindow = login;


        //OnStartupExtracted();
        //OnStartupExtracted1();

        //private void OnStartupExtracted1()
        //{
        //    //            var value = 
        //    //                @"
        //    //                    <Employee>
        //    //                        <Name>Johnny Alibuyog</Name>
        //    //                        <Address>#13 Rosal Street Tres Hermanas Village, Mayamot, Antipolo City</Address>
        //    //                    </Employee>
        //    //                ";
        //    //            var xmlDoc = System.Xml.Linq.XDocument.Parse(value);

        //    //Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        //    //// authentication (login / registration)
        //    //var authView = IoC.Container.Resolve<AuthenticationView>();
        //    //var authViewModel = IoC.Container.Resolve<AuthenticationViewModel>();
        //    //authView.DataContext = authViewModel;
        //    //authView.ShowDialog();
        //    //if (!authViewModel.Authentic)
        //    //    this.Shutdown(1);

        //    //// splash screen
        //    //var splashView = IoC.Container.Resolve<SplashScreenView>();
        //    //var splashViewModel = IoC.Container.Resolve<SplashScreenViewModel>();
        //    //splashViewModel.Licensee = "Raquel Octera Corp";
        //    //splashViewModel.Plugins = new[] { "NHibernate", "Ninject", "log4net" };
        //    //splashView.DataContext = splashViewModel;
        //    //splashView.Show();

        //    //// when loading is finish, show main window
        //    //loadingTask.ContinueWith(task =>
        //    //{
        //    //    var mainView = IoC.Container.Resolve<MainView>();
        //    //    var mainViewModel = IoC.Container.Resolve<MainViewModel>();
        //    //    mainView.DataContext = mainViewModel;
        //    //    this.MainWindow = mainView;
        //    //    this.MainWindow.Loaded += (sender, args) => splashDialog.Close();
        //    //    this.MainWindow.ShowDialog();
        //    //    this.Shutdown(1);
        //    //}, TaskScheduler.FromCurrentSynchronizationContext());

        //    //loadingTask.Start();

        //    //TestData.GenerateUser();

        //    //var authDialog = IoC.Container.Resolve<AuthenticationDialogService>();
        //    //authDialog.ShowDialog();

        //    //Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;


        //    this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        //    TestData.GenerateUser();

        //    // authentication (login / registration)
        //    var authDialog = IoC.Container.Resolve<AuthenticationDialogService>();
        //    authDialog.ViewModel.Login.Username = "johnny.alibuyog";
        //    authDialog.ViewModel.Login.Password = "slow_dance";
        //    authDialog.ShowModal();
        //    var actionResult = authDialog.ViewModel.ActionResult;
        //    if (actionResult == null || actionResult.Value == false)
        //    {
        //        this.Shutdown(1);
        //        return;
        //    }

        //    // splash screen
        //    var splashDialog = IoC.Container.Resolve<SplashScreenDialogService>();
        //    splashDialog.ViewModel.Licensee = "Raquel Octera Corp";
        //    splashDialog.ViewModel.Plugins = new[] { "NHibernate", "Ninject", "log4net" };
        //    splashDialog.Show();

        //    // loading
        //    Task.Factory.StartNew(() =>
        //    {
        //        // generate data
        //        TestData.GenerateData();
        //    })
        //    .ContinueWith(task =>
        //    {
        //        var mainDialog = IoC.Container.Resolve<MainDialogService>();
        //        this.MainWindow = mainDialog.View;
        //        this.MainWindow.Loaded += (sender, args) => splashDialog.Close();
        //        this.MainWindow.ShowDialog();
        //        this.Shutdown(1);
        //    }, TaskScheduler.FromCurrentSynchronizationContext());

        //    //if (actionResult.HasValue && actionResult.Value)
        //    //{
        //    //    // splash screen
        //    //    var splashDialog = IoC.Container.Resolve<SplashScreenDialogService>();
        //    //    splashDialog.ViewModel.Licensee = "Raquel Octera Corp";
        //    //    splashDialog.ViewModel.Plugins = new[] { "NHibernate", "Ninject", "log4net" };
        //    //    splashDialog.Show();

        //    //    // loading
        //    //    Task.Factory.StartNew(() =>
        //    //    {
        //    //        // generate data
        //    //        TestData.GenerateData();
        //    //    })
        //    //    .ContinueWith(task =>
        //    //    {
        //    //        var mainDialog = IoC.Container.Resolve<MainDialogService>();
        //    //        this.MainWindow = mainDialog.View;
        //    //        this.MainWindow.Loaded += (sender, args) => splashDialog.Close();
        //    //        this.MainWindow.ShowDialog();
        //    //        this.Shutdown(1);
        //    //    }, TaskScheduler.FromCurrentSynchronizationContext());
        //    //}
        //    //else
        //    //{
        //    //    this.Shutdown(1);
        //    //}
        //}

        //private void OnStartupExtracted2()
        //{
        //    Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        //    var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        //    var authenticationTask = new Task<Nullable<bool>>(() =>
        //    {
        //        var authDialog = IoC.Container.Resolve<AuthenticationDialogService>();
        //        authDialog.ShowModal();

        //        Thread.Sleep(500);
        //        return authDialog.ViewModel.ActionResult;
        //    });

        //    var splashTask = authenticationTask.ContinueWith<Window>(task =>
        //    {
        //        if (task.Result == null || task.Result == false)
        //            this.Shutdown(1);

        //        var splash = IoC.Container.Resolve<SplashScreenDialogService>();
        //        splash.ViewModel.Licensee = "Raquel Octera Corp";
        //        splash.ViewModel.Plugins = new[] { "NHibernate", "Ninject", "log4net" };
        //        splash.View.Show();

        //        Task.Factory.StartNew(() => TestData.GenerateData());

        //        Thread.Sleep(5000);

        //        return splash.View;
        //    }, scheduler);

        //    var mainTask = splashTask.ContinueWith(task =>
        //    {
        //        if (task.Result == null)
        //            this.Shutdown(1);

        //        var mainDialog = IoC.Container.Resolve<MainDialogService>();
        //        this.MainWindow = mainDialog.View;
        //        this.MainWindow.Loaded += (sender, args) => task.Result.Close();
        //        this.MainWindow.ShowDialog();

        //        this.Shutdown(1);
        //    }, scheduler);

        //    authenticationTask.Start(scheduler);
        //}
    }
}
