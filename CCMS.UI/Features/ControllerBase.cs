using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using CCMS.Data;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using Common.Logging;
using NHibernate;
using ReactiveUI;

namespace CCMS.UI.Features
{
    public class ControllerBase<TViewModel> where TViewModel : ViewModelBase
    {
        private ILog _log;
        private TViewModel _viewModel;
        private IMessageBus _messageBus;
        private IMessageBoxService _messageBox;
        private ISessionProvider _sessionProvider;

        internal virtual ILog Log
        {
            get
            {
                if (_log == null)
                    _log = LogManager.GetCurrentClassLogger(); //log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

                return _log;
            }
        }

        internal virtual TViewModel ViewModel
        {
            get { return _viewModel; }
        }

        internal virtual IMessageBus MessageBus
        {
            get
            {
                if (_messageBus == null)
                    _messageBus = IoC.Container.Resolve<IMessageBus>();

                return _messageBus;
            }
        }

        internal virtual IMessageBoxService MessageBox
        {
            get
            {
                if (_messageBox == null)
                    _messageBox = IoC.Container.Resolve<IMessageBoxService>();

                return _messageBox;
            }
        }

        internal virtual ISessionFactory SessionFactory
        {
            get { return this.SessionProvider.SessionFactory; }
        }

        internal virtual ISessionProvider SessionProvider
        {
            get
            {
                if (_sessionProvider == null)
                {
                    _sessionProvider = IoC.Container.Resolve<ISessionProvider>();
                }

                return _sessionProvider;
            }
        }

        internal virtual void DispatcherInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public ControllerBase(TViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
