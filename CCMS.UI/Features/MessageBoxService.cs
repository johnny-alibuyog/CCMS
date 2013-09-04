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

        private MessageBoxResult ResovleResult(System.Windows.MessageBoxResult result)
        {
            switch (result)
            {
                case System.Windows.MessageBoxResult.OK:
                case System.Windows.MessageBoxResult.Yes:
                    return MessageBoxResult.OK;
                default:
                    return MessageBoxResult.Cancel;
            }
        }

        public MessageBoxResult ShowError(string message, string caption = "Error")
        {
            return ResovleResult(MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error));
        }

        public MessageBoxResult ShowError(string message, Exception ex, string caption = "Error")
        {
            _log.Error(message, ex);
            return this.ShowError(message, caption);
        }

        public MessageBoxResult ShowQuestion(string message, string caption = "Confirmation")
        {
            return ResovleResult(MessageBox.Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Question));
        }

        public MessageBoxResult ShowInformation(string message, string caption = "Information")
        {
            return ResovleResult(MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information));
        }

    }
}
