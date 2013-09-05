using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features
{
    public interface IMessageBoxService
    {
        void Warn(string message, string caption = "Error");
        void Warn(string message, Exception ex, string caption = "Error");
        MessageBoxResult Confirm(string message, string caption = "Confirmation");
        void Inform(string message, string caption = "Information");
    }
}
