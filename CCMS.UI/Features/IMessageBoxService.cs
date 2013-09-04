using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features
{
    public interface IMessageBoxService
    {
        MessageBoxResult ShowError(string message, string caption = "Error");
        MessageBoxResult ShowError(string message, Exception ex, string caption = "Error");
        MessageBoxResult ShowQuestion(string message, string caption = "Confirmation");
        MessageBoxResult ShowInformation(string message, string caption = "Information");
    }
}
