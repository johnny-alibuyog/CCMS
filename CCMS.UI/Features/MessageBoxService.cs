using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Common.Logging;

namespace CCMS.UI.Features
{
    public class MessageBoxService : IMessageBoxService
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger(); //log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Warn(string message, string caption = "Error")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Warn(string message, Exception ex, string caption = "Error")
        {
            _log.Error(message, ex);
            this.Warn(message, caption);
        }

        public MessageBoxResult Confirm(string message, string caption = "Confirmation")
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case System.Windows.MessageBoxResult.Yes:
                    return MessageBoxResult.OK;
                default:
                    return MessageBoxResult.Cancel;
            }
        }

        public void Inform(string message, string caption = "Information")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
